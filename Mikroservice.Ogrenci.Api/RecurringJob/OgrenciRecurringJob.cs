using AutoMapper;
using Hangfire;
using Microservice.Ogrenci.Application.Contracts.IRepositories;
using Microservice.Ogrenci.Domain.Entities;
using Mikroservice.Ogrenci.Api;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

public class OgrenciRecurringJob
{
    private readonly IOgrenciRepository _ogrenciRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    public OgrenciRecurringJob(
        IOgrenciRepository ogrenciRepository,IUnitOfWork unitOfWork,
        IMapper mapper,
        HttpClient httpClient)
    {
        _ogrenciRepository = ogrenciRepository;
        _mapper = mapper;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public static void VeriTabaniGuncellemeJob()
    {
        RecurringJob.AddOrUpdate<OgrenciRecurringJob>(
            "OgrenciGuncellemeJob",
            x => x.OgrencileriSonGuncellemeTarihiyleGuncelle(),
            Cron.Daily(04, 00),
            new RecurringJobOptions {
                TimeZone = TimeZoneInfo.Local
            });
    }

    public async Task OgrencileriSonGuncellemeTarihiyleGuncelle()
    {
        var datetime = DateTime.Now.AddDays(-1);

        var formatInfo = new CultureInfo("en-US").DateTimeFormat;
        formatInfo.DateSeparator = "-";

        string tarih = datetime.ToString("u", formatInfo);

        string requestJson = "{'serviceName': 'GetPersonStudents','serviceCriteria':{'SonGuncellemeTarihi':" + "'" + tarih + "'" + "} }";

        await IstekGonder(requestJson);
    }

    public async Task IstekGonder(string requestJson)
    {
        var url = "https://ubys.sivas.edu.tr/framework/Integration/ServiceCaller/Report";

        var base64String = Convert.ToBase64String(
            Encoding.ASCII.GetBytes("sbtu:sivas!52*"));

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Basic", base64String);

        _httpClient.Timeout = TimeSpan.FromMinutes(10);

        ServicePointManager.SecurityProtocol =
            SecurityProtocolType.Tls |
            SecurityProtocolType.Tls11 |
            SecurityProtocolType.Tls12;

        var stringContent = new StringContent(requestJson, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(url, stringContent);

        var responseBody = await response.Content.ReadAsStringAsync();

        if (responseBody != "\"Access is denied.\"" &&
            !responseBody.Contains("maksimum çağrıla sayısına ulaştı"))
        {
            var ogrenciler = JsonConvert.DeserializeObject<List<Ogrenci>>(responseBody) ?? [];

            foreach (var ogrenci in ogrenciler)
            {
             
                var guncellenecekOgrenci =
                    await _ogrenciRepository.GetOgrenciByOgrenciProgramId(ogrenci.ogrenciprogramid);

                if (guncellenecekOgrenci == null)
                {
                    await _ogrenciRepository.AddAsync(ogrenci);
                }
                else
                {
                    _mapper.Map(ogrenci, guncellenecekOgrenci);
                    _ogrenciRepository.Update(guncellenecekOgrenci);
                }

          

            }
            await _unitOfWork.CommitChangesAsync();
        }
        else
        {
            await Task.Delay(TimeSpan.FromMinutes(40));
            await IstekGonder(requestJson);
        }
    }
}
using AutoMapper;
using Hangfire;
using Microservice.Personel.Application.Contracts.IRepositories;
using Microservice.Personel.Domain.Entities;
using Mikroservice.Personel.Api;
using Newtonsoft.Json;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

public class PersonelRecurringJob
{
    private readonly IPersonelRepository _personelRepository;
    private readonly IMapper _mapper;
    private readonly HttpClient _httpClient;
    private readonly IUnitOfWork _unitOfWork;
    public PersonelRecurringJob(
        IPersonelRepository personelRepository,IUnitOfWork unitOfWork,
        IMapper mapper,
        HttpClient httpClient)
    {
        _personelRepository = personelRepository;
        _mapper = mapper;
        _httpClient = httpClient;
        _unitOfWork = unitOfWork;
    }

    public static void VeriTabaniGuncellemeJob()
    {
        RecurringJob.AddOrUpdate<PersonelRecurringJob>(
            "PersonelGuncellemeJob",
            x => x.PersonelleriSonGuncellemeTarihiyleGuncelle(),
            Cron.Daily(04, 00),
            new RecurringJobOptions {
                TimeZone = TimeZoneInfo.Local
            });
    }

    public async Task PersonelleriSonGuncellemeTarihiyleGuncelle()
    {
        var datetime = DateTime.Now.AddDays(-1);

        var formatInfo = new CultureInfo("en-US").DateTimeFormat;
        formatInfo.DateSeparator = "-";

        string tarih = datetime.ToString("u", formatInfo);

        string requestJson = "{'serviceName': 'GetWorkers','serviceCriteria':{  'GetPersonEncryptedId':true,  'SonGuncellemeTarihi':" + "'" + tarih + "'" + "} }";

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
            var personeller = JsonConvert.DeserializeObject<List<Personel>>(responseBody) ?? [];

            foreach (var personel in personeller)
            {
             
                var guncellenecekPersonel =
                    await _personelRepository.GetPersonelByUsername(personel.username!);

                if (guncellenecekPersonel == null)
                {
                    await _personelRepository.AddAsync(personel);
                }
                else
                {
                    _mapper.Map(personel, guncellenecekPersonel);
                    _personelRepository.Update(guncellenecekPersonel);
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
using k8s;
using k8s.KubeConfigModels;
using k8s.Models;

namespace StackUnderFlow.Infrastructure.Kubernetes;

public class KubernetesService
{
    private readonly IKubernetes _client = new k8s.Kubernetes(
        KubernetesClientConfiguration.BuildConfigFromConfigObject(new K8SConfiguration
        {
            ApiVersion = "v1",
            Clusters = new[]
            {
                new Cluster
                {
                    Name = "HERMES",
                    ClusterEndpoint = new ClusterEndpoint
                    {
                        Server = "https://hermes-dns-z7fio5j8.hcp.uksouth.azmk8s.io:443",
                        CertificateAuthorityData =
                            "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUU2VENDQXRHZ0F3SUJBZ0lSQU1VUWVXZ2VxSHVUWWpJRUthTFBWdll3RFFZSktvWklodmNOQVFFTEJRQXcKRFRFTE1Ba0dBMVVFQXhNQ1kyRXdJQmNOTWpRd05qRTBNRGN6TnpRMVdoZ1BNakExTkRBMk1UUXdOelEzTkRWYQpNQTB4Q3pBSkJnTlZCQU1UQW1OaE1JSUNJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBZzhBTUlJQ0NnS0NBZ0VBCjF1TUJTbHRsSmdnUEtJKzYvNkhCZHVWdmVSc0VnZ0FyTG9JcVdpMnBKQk04N0ZRU1hzUWE1SWJENVJ1VTNBbUEKYTgxLzVOb2JYczZadk9tbXM5aXdTV0NKNlZqQXR6emtnbmRlOGd2bTJCZDk1L3JOSndUTlB3dE9OVmdDVW9vNApzN084OU1Da2dHZmxZaUZvSHVSR1FPTXNqcHI4Si9iSTA3T1dnZWp3YzlqUHJMYVJXZ2V2TXZyRU1kaHh4UlFQCkxzazdaSkpNOFpoY1VLQWYxOFdpd3FHbml3SGpWOS94RE5yUnFFUUlvUUVldFRBYnZNYmJOVld0UWpmQ0JadDQKU08rSllxbk5sZWw0YXFSSnNmcHI0UFRJa3FiUzdSNEZIMHJhN1J0dWZoYzFXd3RTWGs0bURLeUhzd1hFZk96WgpqZVVUbUxKOW8rakwyTGpGbVZ6SWtma0RjLytJZVVndWFRQTUrYTFlTm43WGpVVkxmNmRaQnNDeXBZcDhPK3B1CmhZYzhCRmMrK3NoUE54bnV5WlhiQ25KbTRXSTc5azBOYlJVMmRGWFJvR2p2czhFaisyRUIvSGhtVGxrMnJrek4Kemxic0NlS24zcHQwSlRrSEcyWnF5Zkc3VGEycHQ3T1NnSXYyQTVzZC9rd2ZuMkVGWVVsbUlTMFZwU3dNampLKwozMWUwZ00wR2hxNnVwUDdlT1ZyamRXaDF3RlFxTFJWUUkwd0g4VE4ybUR0aUZwWnZZdVM3LzlFei90VUJ4RzRNCm5laVp6QUN1RXIrTW1zNUE3ZVNOU3gvTTF0MjE2dlAveVZYSjFVdFE4ZDlzV2pGK0hEUmxBbTRzWHNISE0xSDIKUWR3SDNTNTFONDdMM09za1YyRHJtNVozQkRic1V2SlBkSG1NY0xxNWg4Y0NBd0VBQWFOQ01FQXdEZ1lEVlIwUApBUUgvQkFRREFnS2tNQThHQTFVZEV3RUIvd1FGTUFNQkFmOHdIUVlEVlIwT0JCWUVGSWZVbXpQcGdld0gvSXRpCmEwaXpRSlQrNFdTNU1BMEdDU3FHU0liM0RRRUJDd1VBQTRJQ0FRQmdpQUhKb2FMaU56WEVVRzByaFM2MUtPdTQKMlN4TEl6TTVTTmJoQW1ycVhPK3BKMGVHeUVyVkt0VjFkL3NRWHNoWDNsSXFQb0RnL3o2aGRiLzNJZFRybWxrUAovZVRyRGJkLzd0QjNxQk5RQk1la2x3eEpzZEdVUnBjQUd1VkV5TkRqOGFoaVFkOVB1Ykg3MDRkOVFiWCtQNzdiCk5SVkRLSDdmckYxTEQ1RXFGcjhMZXU3bExRRnBra0IxVzZvUmtUOXBKUGhGR1Y4NjhFNVQ2RWdKditPV2tYT08KNTFwWmNBblp6bWpoMkhVQ0xCdHBMUThpVlN2cjRlcDdzUy8zMUlBay90aWtsVGk4L1lsZjYrZzRaSThEV2VobwpyRnhiRm5ua0hWT2ZDUDdUVkYrMVVjVjNBNTQwWm5vN1g4ZjVTQllaWGppNWtseE5kUEZuazhWdlVMcGttYWl2CjJHT29RREJicmVodFRpaVRIdWVmYU95a3hyQTVmdWRBQzd3MllnTG1hV0JzYnViQy9GazdHZ1pOZWdnMzFYNTcKRVFTSExJT3FhWkUxNi8vUndOYllTdVlOaVdWTnFIOGFXK3NnZ1VPVGtFTHNhZzZRbzRIZUd3TnhMVkJRaERORQpVbWtUM2o4ak5NWmw3MTlGKzFVamhTTzlENXl6bnhWK3BnYUcwMUZneG43RkNySWVSVHhYclRUV2VETExrZGFSCjRiRzh4VWk5WjI0NFB0WFozcmVMczVNNUZTelRGYXF1WjNzYTI2WU4rWlA5dHhva3pNVXRxaGFVeXMrS1Y0MDEKb3g5MHIyaDF1Y3Jlcnl3RUJyNVdyUXREbTJkYlNmaDZKS1lXRFNHNVNnVmFWU0NValdmZnNaSXhuNHRMU2F2Ugp2b2orWDMvV2FIdXFNcElmTFE9PQotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0tCg=="
                    }
                }
            },
            Contexts = new[]
            {
                new Context
                {
                    Name = "HERMES-admin",
                    ContextDetails = new ContextDetails
                    {
                        Cluster = "HERMES",
                        User = "clusterAdmin_StackUnderflow_HERMES"
                    }
                }
            },
            CurrentContext = "HERMES-admin",
            Kind = "Config",
            Users = new[]
            {
                new User
                {
                    Name = "clusterAdmin_StackUnderflow_HERMES",
                    UserCredentials = new UserCredentials
                    {
                        ClientCertificateData =
                            "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUZIakNDQXdhZ0F3SUJBZ0lSQUpvZXp6WElJT1dhTTV1Q3RlLy9RMFF3RFFZSktvWklodmNOQVFFTEJRQXcKRFRFTE1Ba0dBMVVFQXhNQ1kyRXdIaGNOTWpRd05qRTBNRGN6TnpRMVdoY05Nall3TmpFME1EYzBOelExV2pBdwpNUmN3RlFZRFZRUUtFdzV6ZVhOMFpXMDZiV0Z6ZEdWeWN6RVZNQk1HQTFVRUF4TU1iV0Z6ZEdWeVkyeHBaVzUwCk1JSUNJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBZzhBTUlJQ0NnS0NBZ0VBL0Rqc2pNZEg1dEw3QXg2VWRvM1oKY24ybWdYbGdjdDRpeTBzL0NRUnlidXBxU091UlhEV1FYN2FzOGc0S1ZmeXp3dDRVYzF6bWd2WWJHUjdpVmpOTQpOelBDUlU0SnZQM1YrbFM2QjkzaDNpRmNOQ3NXaHRaczhwL3FtNnJzeG13MlZObUh6NTFoVTFrbXZGRXBJSUZUCmJjdkszS3A4ZXBQbzZNTTZTUFk5S1U1b0NwV1lTS1ZXOHVxVUZOeXZFR3d2RUx6R2lyV0cyL3FSd0FHd0t2RVAKdFlud2xFZHk5M3p2aVpsbjhEeHFjOUxTRmh4cjRJQ21STC9XQ1Y0aUFZMVdyRmZvdUhyMmFRcXdlUmtTbDBvaQo0aEZ2Ny8vdU1KcVJGdDNkZVVNdnZBNjVIVjRLTW5HdDZBUmxsVEZLR3l2MG96dS80TU50Qm15bDBrU1JVcko4CnEyU2ZtQmJISml3dlRTZGxqRHJjTDBMeDJBeEhXazdnbFcwUCt0QzBtMUp1RFJiYm9uVnpBVnN4MW1xY3hPM00Kd1k2L1ZwU2FDUWE0S2Q5TmpVakEvcHlZVEl2VFJnbU1yMlhtUnRiOVh0QjRoajBOYmJBZlIrL1NlWkdxRDhFTApYSmJmUDRkeUkzdWdZS1VaYkFXRmdQVnRGYUkxN283cVI3M1BTQUtNS1QvRTQxZ2hPOCtVdVRLSjExZC82UFozCkcydUdxSUxCc3N2SFFvY0h0Q3YvUk40UmY1NkJXM0J0UElaV2JvblYraFBYMllWaUZ1OUZseDNqYXFVdFZDdGgKdWFvN29zekgxdW9vS1lFTEJ1SHM5eDgvNDc5LzVaUGZJNEN6Zi9SYUpkNDUrLzBZV1lIUGhPL1ZONWx4MmFrYgpKeHJhcmRWQzd4R0ZvTDM0dUhDYjJZc0NBd0VBQWFOV01GUXdEZ1lEVlIwUEFRSC9CQVFEQWdXZ01CTUdBMVVkCkpRUU1NQW9HQ0NzR0FRVUZCd01DTUF3R0ExVWRFd0VCL3dRQ01BQXdId1lEVlIwakJCZ3dGb0FVaDlTYk0rbUIKN0FmOGkySnJTTE5BbFA3aFpMa3dEUVlKS29aSWh2Y05BUUVMQlFBRGdnSUJBSVU2RWxVMTBYUWhRWGMreDhxQgo4RHM2NTAyRHhjdllzeCtINEtDTzVTNWh2MHdrWkhQbVUrQzVBRkcxU2VQdUxYSG1mazhxVDI5K0c5SzM4ajJKCmhGNzlUN2hhekp0bTA2aGJTanRWcVlCckFWWmlpVFpGOUpmbHJkQ3FHc1NXK2ZJUXdRc2dhOExOSm9WOFl6N2UKYVhRaDZmZnBLRFlyYVhIbGd2RkhJOEdBL1V0Zml5eTJCZmV6TFhqcWhzSFdFTVRSNzdkZzJwZFowYkpkK0JzbAo3WUlOS1lhckVJVWFEaFB6V2dseTkrMWRuQ0hlUmEyL0Zucmgya1lYbkFWNDZCRzVQWHdEUDB4SUJsa1h3N3ZSCit3YlNXM3RiYkJPanZiT2QvWlBuYWxEek5DUFF1YXRUbk5rczgvaU1rbnRmQ1BsdUtSOENvR2toekJLWkE3NkkKZndiZGQvRnZ6MjRsdit5ZG5zV0l4cVVZcWlIR3k4N0UyRVl5ZFJsSXFQNmNjYmNyTXNLeHFHcmFRRHdhRWIyOAo5eEwrL0RmbDJuWjRSTjg2MlVYeE5GcGErRDBOSGJGR1ovOHFDTENwbUJkK2FGYnRUOEdSVWtMNXNHN3Q4VXdnCktLQk1KZ1IxbGdKZ215cm94MXA4Z0RWVjdDUGUrOFlreTV5UFFwMHZHei9jazN1UVRjWC95dWJCQXRvN3RRN08KMHZoMCsremRzWTY4OUNyMWsyZkRsWTlQbit4RFlxNkNvQVhhZ3NzUmJCQ3JDWld6QXBvRzJhQTRmTUZMZ0NQVQpUbTUwV0VGejJJNkY5SFh2V1JlM0dMQk1WRUhyRUdrRUlPZDRBQVNtM0JERC9zWHhIVFM2M2ExWXBjY2xWQ1BmCjZ0RVZFZVJab0htM3c0ZWt2alYrTHp0VgotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0tCg==",
                        ClientKeyData =
                            "LS0tLS1CRUdJTiBSU0EgUFJJVkFURSBLRVktLS0tLQpNSUlKS0FJQkFBS0NBZ0VBL0Rqc2pNZEg1dEw3QXg2VWRvM1pjbjJtZ1hsZ2N0NGl5MHMvQ1FSeWJ1cHFTT3VSClhEV1FYN2FzOGc0S1ZmeXp3dDRVYzF6bWd2WWJHUjdpVmpOTU56UENSVTRKdlAzVitsUzZCOTNoM2lGY05Dc1cKaHRaczhwL3FtNnJzeG13MlZObUh6NTFoVTFrbXZGRXBJSUZUYmN2SzNLcDhlcFBvNk1NNlNQWTlLVTVvQ3BXWQpTS1ZXOHVxVUZOeXZFR3d2RUx6R2lyV0cyL3FSd0FHd0t2RVB0WW53bEVkeTkzenZpWmxuOER4cWM5TFNGaHhyCjRJQ21STC9XQ1Y0aUFZMVdyRmZvdUhyMmFRcXdlUmtTbDBvaTRoRnY3Ly91TUpxUkZ0M2RlVU12dkE2NUhWNEsKTW5HdDZBUmxsVEZLR3l2MG96dS80TU50Qm15bDBrU1JVcko4cTJTZm1CYkhKaXd2VFNkbGpEcmNMMEx4MkF4SApXazdnbFcwUCt0QzBtMUp1RFJiYm9uVnpBVnN4MW1xY3hPM013WTYvVnBTYUNRYTRLZDlOalVqQS9weVlUSXZUClJnbU1yMlhtUnRiOVh0QjRoajBOYmJBZlIrL1NlWkdxRDhFTFhKYmZQNGR5STN1Z1lLVVpiQVdGZ1BWdEZhSTEKN283cVI3M1BTQUtNS1QvRTQxZ2hPOCtVdVRLSjExZC82UFozRzJ1R3FJTEJzc3ZIUW9jSHRDdi9STjRSZjU2QgpXM0J0UElaV2JvblYraFBYMllWaUZ1OUZseDNqYXFVdFZDdGh1YW83b3N6SDF1b29LWUVMQnVIczl4OC80NzkvCjVaUGZJNEN6Zi9SYUpkNDUrLzBZV1lIUGhPL1ZONWx4MmFrYkp4cmFyZFZDN3hHRm9MMzR1SENiMllzQ0F3RUEKQVFLQ0FnQjVKUVpKWC9aaklmYzZ1bGRvMGgwZFpzaXc2NUd0MnBBdndRYVgyREQyb1ZWSGpRNFdrZ3UwVFZPbwpONkl6UnRzNHY5NW13cnBkTU1RM1BxUkw3dnV1a0FmQnJnZnpaS0NBU20zSUZZVEZZcFNjNGcxQjJvQWQwVDJvClkyS3lzNHN0R2dhbmE5b3haR0s4bE9jQ0c5dnNvclBmWld2QW5JYUVOakVxbGtzdUtlRERKTE11UVd3UDVTZ1gKRlNCbzdPMTJScFcrVGc1bVhtWGZLWEJxVDdyUjRWMVNlemIzTkdVQlhGT1dDZzYvYndWRlpZVlRJYmU2MGNHRwpZbFpIRkpJSzJYYUlGVmVCZ0liZXczSXdoSVZZdkROZHZUbTAwcWpacU5zVndYV09DQ3hWdExPRkl5RDZqaElVCnBNWUpZd1Z2MmVVQm1od2x0ODIxdndWbUNTRGpaYW1VTUNiMXZNMTlRMTVrQURLK2tVcTh3bTMvUlkxSHFGL1oKckJJQXJmTldYU0xtSWF5dDlXbC92amgzSG0xQ2Eya3VDNEdnRE80YWZOR0JXNzdMbE1GeG5XWW1aZVFSR1N6Ngp4UHY1dFdnRDc0aVkxSnRvVUpQNDV5VWJMMm12eXVBNndGNkZJT281MGI1TXhyUHhidTZWQkN0OGNyL2kvNzF2CjE0UFJvUEVXNVpaa01zcjBGMUErN0FNcUpLM3dXSFpDL0dWR1RSaGxMNzFMZzh4RERYdVIxZXlyaWJ4ejllaVgKQUVkUUFkM0NyRXlVSjNyR1F2UDI5YlI4SVVxdWVUbU0xTGE5cUhQSnk4UnRZNG9MbUdnS3lCVE04YmRQZmpnZAo2NVE2TXl1dHFiNkE2b2xTd0tNNHczTUhCZkR5R3FUUTFwQWlTc0lRUmFyeDBYR2lNUUtDQVFFQS9ncFFLSXlUCkNvUlRyMjZXWDZkVnVrZ1RMRTJIMnEveFp1NXNjc0hGa3dsNkRjcnpESmNoaU8wazZtcU1aYmhNejJzS29EN1MKcjJQRFhpbnhMRUQ5OTA5WXZXLzBvQk5CVC9GWGlWcWNRL1ZTTWcwR0YwbnhENGp1aTgvdXlmUE42SzYyaHNrMgpQcFBSeWVDSmNncEt3TTRhUFJlcjZ5RXA3cE15Yko1eTU3RjVjdSt3RXpmRTFkUWczYlZsK3BuT0VBMUx3UUJWCnJzYTk3TjRMSFNPRGoxRnh4SVlYSEFleDdSd2I4SlZTeTlKaC9JZGpKRlBUK09EREZsTHAweW1UYmF3cXFuMFQKdHBELys0UjZJTFUwcUIrL3IwbXFXcitraVZlZ0g4N0lmaEg1emw0YkFBWldpQ1ZJMW1reEVzMEZ2WE9rd3REdApkb3pmK2NPVnFSMytsd0tDQVFFQS9pc0ZVNFVhZGdLSHZBV0tDMzM5R3hiY05NYnlVa0dML0pJeGNaMnlOVkQ5CkJEVTA4WHNsdXA5NkhwNW9LbHNqWmlaVjFwZnJCelNYRk54aDY0cEZlL0pKcGRMN0xFT2R3LzdDYlhIWUw5R1QKTXVWbXVhTjZoNk9QUWs5cVZYaitQVitETU9SNmFPd0JVTkhPdFQ0RlBXTk9FTWRFci9NSXcrVHIvTXNTK0FvYwpkNEdHTWluYm92MVR2andmcTBUalFWMEFCZHNyejJEU25BL1dUSjZqckp6RGpqclI1bGh6UldoNTY5QVVLd25XCmdlSHZ5NG5rM1dSdEZqdnBQNTZiU0ZwRDBwQVpQRU92dnliZUpaQXZXNmdITE5OeGsrR3BYN0FZVnFaSklWTUcKQnAycEhpUjcvYSt4ak1wb1JDSU5DVm9mL3JVa2dPVStFNk0xSDViUExRS0NBUUFnWmVSUEo0NWhHdnNwTm10TApDNll0T1ovb1dJTG82dU5ZZ3pPbGR2emhnYVhsT0dyQ3drdHVrUC9TUFlCbVFKamJJd1daNWlrc1lRYTdiWkhxCldPZUtzTDNhZXZxeHA0TCsxUUthNjhsZUNWMVFNTVVFRjFQODdUT3U5UGU1SGJTMjVnRTFNMWdOcHdCc1JJeUMKMUxrdjJaa1REWC9KWHROZ0w3bVFqS3lPeTkxM1FRWXRqVUUzRy9TTkVlTk5rR3Z0TkNUakdrM1RHbG1DYWRiMQpBbUIyMktZdnNBSW1ldnpBQm9PeHJQbUFNUFo2SkRJS29mNDRrVUdRQ1ViMHZTcW1JL3pVL1ZxVVArM0ZmK0dyClpqQ3UrRTJUNTBzb2c2UlNON0NlSGRzSDZQYjQwSnVVNXpvRjV1dGhITTA3WHBaSzBRTEVMYXo3SDRBNDlDNlUKQU5WRkFvSUJBQlRvVnd6U3d5VjMrZEJtcUQ5ZndzVktzUnlLVFA2bE02MjRIT2Nhc01FZ0EyQW9QRTJzOHFLUwpZY3BJLzRxVWFxb0pkMEFxeVFPVHVPWFhaeHFvQ2lVeS9nbnMvQXBkR1lvNDE2ZUhHT0IrSGR5dThDOVBHbEkwCkN5SEtSSlg1V1BpVmRjWTgxVER5VVAxajlOd0YyUXArczdvL21nL2JMeHBtSjE5cEdRdGNVWkVuRDNIcTdZMVAKeDJhN3JXTTZUSE5oQWhKNVAvWlVJWjhDMG44RkFiTytSWHZhck0rRThSNkJoSjM0Uk9CeGM3ckZhaU5WR1lWNApzRnlHMng0SG53QnBwSENhdCtpMExLM050YkpqVlBIMk5YampmOUZKVzlScU9PWlAwVzR6VlNpUUtZdzROaGJkCkwvN0QyNlhYSXpsa3Q0TU5SWTV3elJWeWRIay92NVVDZ2dFQkFQQjNMcUNvZzdSdlhCQTdvbFJSa0s0VVJJQ0kKb2tjN3k0Q3p5ekJ1azg2M2xhb0dRU1l4TytuU2FYWStFQk9JNjNWYzFyT29SVmVMUFhSMGtJOFV4QjdmbTRkMQpkK2djb2t3SG1tSDg2aURFcGtqSVhkbWhYRmgremwrVm9rb3d1YUxpdnFoMWtZUk1ia0VaSVRieC9Ib3pVaHhXCnlDS2UyekZHZkFpY1J6MHR0aDhJR1NINVVqaDBCNTd0K2U2dTB4UHphVVM0WjI0NDg0MDY5TzlrTG1INTJ6THMKZ0g2akNwVTdSZGRIUlBSekxDT2VpZDlxOFlkcVFPSGJoUjBnaXRuNVRKRWRYMTVCNW9CcHFKVk50cTlpYUxNQwo2YVIrL3JiRjJxTGEvU01uOUZWVmVFOHIvdGZqN014YkNSWXU0KzNPUktqRkZFbEc0eU1OdW1PVmdkZz0KLS0tLS1FTkQgUlNBIFBSSVZBVEUgS0VZLS0tLS0K",
                        Token =
                            "7uy51kcjn7pxv35iodk4czk97ouc0yx06a8bx4r9b9jezpcafdpdvz2tasxrcj6ens314oerj798skw95zd357u9bh0icwhuxe6jby8gsxdbvonkrd92nyiq3f60cbfh"
                    }
                }
            }
        })
    );

    public async Task<string> ExecutePythonScript(
        string namespaceName,
        string scriptContent,
        Action<string> notifyCallback
    )
    {
        var jobName = Guid.NewGuid().ToString();
        try
        {
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta { Name = jobName },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new()
                                {
                                    Name = "python-container" + Guid.NewGuid(),
                                    Image = "python:3.9",
                                    Command = new List<string>
                                    {
                                        "bash",
                                        "-c",
                                        $"echo '{scriptContent}' | python"
                                    }
                                }
                            },
                            RestartPolicy = "Never"
                        }
                    }
                }
            };

            notifyCallback("Creating job...");
            var createdJob = await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
            if (createdJob == null)
            {
                throw new Exception(
                    $"Failed to create job with name {jobName} in namespace {namespaceName}"
                );
            }

            await Task.Delay(5000);

            notifyCallback("Job created. Retrieving pod name...");
            var podName = await GetJobPodNameAsync(namespaceName, jobName);
            if (podName == null)
            {
                throw new Exception(
                    $"Failed to get pod name for job {jobName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Retrieving logs...");
            var logs = await GetPodLogsAsync(namespaceName, podName);
            if (logs == null)
            {
                throw new Exception(
                    $"Failed to get logs for pod {podName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Deleting job...");
            DeleteJob(namespaceName, jobName);
            return logs;
        }
        catch (Exception ex)
        {
            throw new Exception($"exception: {ex.Message}");
        }
    }

    public async Task<string> ExecuteCsharpScript(
        string namespaceName,
        string scriptContent,
        Action<string> notifyCallback
    )
    {
        var jobName = Guid.NewGuid().ToString();
        try
        {
            var job = new V1Job
            {
                ApiVersion = "batch/v1",
                Kind = "Job",
                Metadata = new V1ObjectMeta { Name = jobName },
                Spec = new V1JobSpec
                {
                    Template = new V1PodTemplateSpec
                    {
                        Spec = new V1PodSpec
                        {
                            Containers = new List<V1Container>
                            {
                                new()
                                {
                                    Name = "csharp-container" + Guid.NewGuid(),
                                    Image = "mcr.microsoft.com/dotnet/sdk:8.0",
                                    Command = new List<string>
                                    {
                                        "bash",
                                        "-c",
                                        $"echo '{scriptContent}' > script.cs && mcs script.cs && mono script.exe"
                                    }
                                }
                            },
                            RestartPolicy = "Never",
                        }
                    }
                }
            };

            notifyCallback("Creating job...");
            var createdJob = await _client.BatchV1.CreateNamespacedJobAsync(job, namespaceName);
            if (createdJob == null)
            {
                throw new Exception(
                    $"Failed to create job with name {jobName} in namespace {namespaceName}"
                );
            }

            await Task.Delay(5000);

            notifyCallback("Job created. Retrieving pod name...");
            var podName = await GetJobPodNameAsync(namespaceName, jobName);
            if (podName == null)
            {
                throw new Exception(
                    $"Failed to get pod name for job {jobName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Retrieving logs...");
            var logs = await GetPodLogsAsync(namespaceName, podName);
            if (logs == null)
            {
                throw new Exception(
                    $"Failed to get logs for pod {podName} in namespace {namespaceName}"
                );
            }

            notifyCallback("Deleting job...");
            DeleteJob(namespaceName, jobName);
            return logs;
        }
        catch (Exception ex)
        {
            throw new Exception($"exception: {ex.Message}");
        }
    }

    private async void DeleteJob(string namespaceName, string jobName)
    {
        var deletedJob = await _client.BatchV1.DeleteNamespacedJobAsync(
            jobName,
            namespaceName,
            new V1DeleteOptions { PropagationPolicy = "Foreground" }
        );
        if (deletedJob == null)
        {
            throw new Exception(
                $"Failed to delete job with name {jobName} in namespace {namespaceName}"
            );
        }
    }

    private async Task<string?> GetJobPodNameAsync(string namespaceName, string jobName)
    {
        var pods = await _client.CoreV1.ListNamespacedPodAsync(
            namespaceName,
            labelSelector: $"job-name={jobName}"
        );
        return pods.Items.FirstOrDefault()?.Metadata.Name;
    }

    private async Task<string> GetPodLogsAsync(string namespaceName, string podName)
    {
        await using var logs = await _client.CoreV1.ReadNamespacedPodLogAsync(
            podName,
            namespaceName
        );
        using var reader = new StreamReader(logs);
        var result = await reader.ReadToEndAsync();
        if (string.IsNullOrEmpty(result))
        {
            throw new Exception(
                $"Failed to get logs for pod {podName} in namespace {namespaceName}"
            );
        }

        return result;
    }
}
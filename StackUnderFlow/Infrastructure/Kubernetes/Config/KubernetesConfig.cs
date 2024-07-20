using k8s;
using k8s.KubeConfigModels;

namespace StackUnderFlow.Infrastructure.Kubernetes.Config
{
    public static class KubernetesConfig
    {
        public static readonly IKubernetes Client = new k8s.Kubernetes(
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
                            Server = "https://hermes-dns-cmfny30b.hcp.francecentral.azmk8s.io:443",
                            CertificateAuthorityData = 
                                "LS0tLS1CRUdJTiBDRVJUSUZJQ0FURS0tLS0tCk1JSUU2VENDQXRHZ0F3SUJBZ0lSQUlxRjJISkdPWU1PUUswK1d4cEd5Q2N3RFFZSktvWklodmNOQVFFTEJRQXcKRFRFTE1Ba0dBMVVFQXhNQ1kyRXdJQmNOTWpRd056RXdNVEl5TlRJM1doZ1BNakExTkRBM01UQXhNak0xTWpkYQpNQTB4Q3pBSkJnTlZCQU1UQW1OaE1JSUNJakFOQmdrcWhraUc5dzBCQVFFRkFBT0NBZzhBTUlJQ0NnS0NBZ0VBCmx0Y2E5OWdaSWxaRE1RTGZ5SXFTNDZkT0xYK2FNc0ZVQ011aXRjWExaTEdPM1dQT2p0UC9mUm1oYzgybUNiNWwKZEFXSFlXdDdkRXNIaUEvaDA5eEFuaUdJZm0vYU9nWCtJZENRUjJpTkwxZGNueEQzSHN0UzIwRlpCdHRxU0w4VAptWjZtQTEzUFNKM3BURmJHemRuRWFnbFhtMFY5bVNXRy9McnFSU1o3UEZYZWF5Rkw4a3VaeDBOajBJdHp0MkJkCkMzeUR1aGJ0d3Exc0NhelZEd05tNFF5QmVRZ1pjY2RVc3FFVHkxQ1JGdWZ1Yi9UZVJKQktSSzhmWVY4UFlhT0YKRlpVN1IzcEJxN05BU0YvenJ2NzVlaldjd2ZNUjhYYnhVZ0Q2VzF1aEFtVjNLSmdMcXlUWUd1dlk4U0lIaFRxUwpmR0c3bnBuQlc4SWVKSmJ3b0lpSmh4K0VrcjlFaWJQQ2xsbXpOR1FjT1c0ZDB1RDMycktiR2hqU1NudW5qYUNKCkRLYkVONFFsblJsNTJxVW85MXZCVEpvaWRzM2pnbTBaaDZhRGgzaFE3eDl0YklhWG5lZW5QelFBQXdib2FVQUIKOTBycWMvZ1ZoUDRDbGhySEJwUWRPdWgxcHF4SjNIQ3dpY041Q1FZVnpMME5XZTl4TlkxRHovcG4yQ3ZGRFZETgpLOVFKcTU0Tk5nUEdReCtCUktyc0xwaFV6U2VOU0h5MGpzbjJUb25qSEk3MW56Tml0ZlhFRTI3MHlaVWRvcGE4CmpoMjFYNjFOZ1N0YklkaUtLU3p1bHAvcVA3bDRKUmhLNXZzb24zK21OUkFCN0Vod1RVcllwWk9hQnp1RHc0cDQKMjNTNjNobHoyQUJ2Vkk4M1JSSStyalFzQm45alZ2VG0wTG9HMlRuYW5PTUNBd0VBQWFOQ01FQXdEZ1lEVlIwUApBUUgvQkFRREFnS2tNQThHQTFVZEV3RUIvd1FGTUFNQkFmOHdIUVlEVlIwT0JCWUVGREhYQWhMZzhrWmV4Z2RqCkNCY05od29RZUJoZ01BMEdDU3FHU0liM0RRRUJDd1VBQTRJQ0FRQ0NxVHdlNkt4ckE1Y2U0K0kwcUYvWnhUaWkKWEI0Y3FsaTlzTXRrejJVYzhkZmpzUWZDUGpBZHFHZVJSbGVWYzNPelhZWDBtc0o4RUJFSzZNNEVKZFlwWkNwSgpSVXlBdTh6L2RBZWlnVlJsVVFZL21yV0c2SmdJcG01TGtqM1kzSjl5eWowRjk1d01YdndJcG8rNXBVV0UwQlBiCndJalQ3QjJtYmQ0LzIwT2d4V0hGK001WkJkVHZReG1IZjJGMDdXRGxINnI4NnFsaXFjdHNjU2IvRGVIOGkwb3AKeDhFdmEyZG0yTnNrV0JYcDZISG5aeGhTR1RIRmREbG44L21ocFptRkFvT0V6MTBOUHZLL0c5KzhMbGI4OThSSgp6K1N0UkJORnVUd1JIZGtLUWVTWG9iVmxoM3FvQnZiYUU3Q1hKeW1peEVVTjIzNUlENnlZTEVpOVFDK3JBS1Q4CmZWcVdKTlBVVzZtemtVR2NUcDJGcGdHSFU4K1c3THFBeGZNRHUwYmloR0pHY1J3aEkrakx3WWI1Y2JoSmg2b2wKL2M1RDBIRWtIdEF1R2pPUUVsZkNVck5hdllYS1pQZ1Z0V29RTlNEWFFtV2FkM0Z4RHhFZE42SEswMjR5cEFtWAp4VURZY2xwS1kwZ09ZN0YvV3pGMVoxNXJGak1VVmVSRHczN0xHVWt3VmxTTDlOUE03ODUxMzVxb2dwOTZ2SGF1CjgwSVRMN202Y3lxWWxEVTQwSkxJMERqVDErTVBVMmlCS0phYWw4SjAxd08wLzBMOXZldSt4UCt1SEZWZ3hVSVAKVlcranNaMVh5QjVQd0pTWlRUczFRRzZiZ3NSektZK0RFdEcxN3p4ZXRXeVM0dy9YTGg2aFJDV0NNbDkxUjdxaQpEMExtNkx3cUo5YkhXcW1xNlE9PQotLS0tLUVORCBDRVJUSUZJQ0FURS0tLS0tCg=="
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
                            User = "clusterAdmin_StackUnderFlow_HERMES"
                        }
                    }
                },
                CurrentContext = "HERMES-admin",
                Kind = "Config",
                Users = new[]
                {
                    new User
                    {
                        Name = "clusterAdmin_StackUnderFlow_HERMES",
                        UserCredentials = new UserCredentials
                        {
                            Token = 
                                "ir415oebua8dw6lm32zw26c8owofmp7u6ofjdjxx8rp0x0amevcdt09llwi1jm82m5ron8meehmphpgfj4am7cmdswvlcuiuv6i5y03tuiwqryxhevhd6w5da6va1ko8"
                        }
                    }
                }
            })
        );
    }
}

using System;
using System.Linq;
using Shouldly;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Modularity;
using Xunit;

namespace Exam.Challenges
{
    public abstract class ChallengesAppServiceTests<TStartupModule> : ExamApplicationTestBase<TStartupModule>
        where TStartupModule : IAbpModule
    {
        private readonly IChallengesAppService _challengesAppService;
        private readonly IRepository<Challenge, Guid> _challengeRepository;

        public ChallengesAppServiceTests()
        {
            _challengesAppService = GetRequiredService<IChallengesAppService>();
            _challengeRepository = GetRequiredService<IRepository<Challenge, Guid>>();
        }

        [Fact]
        public async Task GetListAsync()
        {
            // Act
            var result = await _challengesAppService.GetListAsync(new GetChallengesInput());

            // Assert
            result.TotalCount.ShouldBe(2);
            result.Items.Count.ShouldBe(2);
            result.Items.Any(x => x.Id == Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f")).ShouldBe(true);
            result.Items.Any(x => x.Id == Guid.Parse("b945f44e-348e-4521-a290-59dccf7a39e7")).ShouldBe(true);
        }

        [Fact]
        public async Task GetAsync()
        {
            // Act
            var result = await _challengesAppService.GetAsync(Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f"));

            // Assert
            result.ShouldNotBeNull();
            result.Id.ShouldBe(Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f"));
        }

        [Fact]
        public async Task CreateAsync()
        {
            // Arrange
            var input = new ChallengeCreateDto
            {
                Name = "a57fd49877804ed8b6df3cfcbea9bf8b0e18747b4e5a400eb14c322148508327c676b379ba384446846804fd738ec4e6743c6a8dbf7d46c18e5b6c92a8685c1f299e4b1374c34984bc26e6209ecad59f2bbad8877dba4ecb8c69179d8e011d4d04a8bb146d4c4babbb4e403dba6a0038b846b259cbb44886bd97373cc9f264589234d2bb66564dad98fd73a83a03a2ebcd366a3972344d559ac6da614c6bc18925677169def148caa593a96a27e049f62eb0465ef764488f8068664abeeedf0fc2b15f5e58754631a56e8d82493eeaa106fc36e8b9a34341b473bddaa7a48aa1220b2e16510e44558a52340ea994d493afcd242cce38405baef426f2f74f2376682ecfcceb9b404480bfa68da7ed4ee32ccbb0fd1ee344449e05f81e1d450f1f51a208db017c4598b8a5a623269a1bfdaf93a31584104c14aebb213e37c2cff1d1303fae9fcd42488e1ed4acffa38852a47526f38be340dd9acf0cd1d69bbca66d1ebe03380e43778225ca183e78214ed59c5d1a894347b0b0e865c481654df948d6f8454b114a7da079ed264f7a23c5ff9a5eb1a227403d8ea7e75ec54dd8f69e8400dd755c4a0ab99bd65d339ab213e10982b749a9410eb4c59bd4291ea453eb237b8490474d6c95e93d1e5ae50a7901113ac4430b448186824ec3c89a661f1a27bd0c4efc4641b4bc5b20959067a4ae8f5e21c7d74fd58bb0e0e7f6d6ed28160d9baaab99425daa845fad88d530b67379a224569a4fadbc0a9458c9247c68bd501f4cb6594492aa1a44e7326625037f3eec91bc344b02885024b6faf8072462a11d1997274517b12dd79a71c45b8e48fee6b274444abca158fb5dd631dafea3dc4023221d45bcaf337454995d11079bfd1b02e21846ab8e03b9cc7c32571f187468783e7548d7b13bab46e022e3b123ec0c2b66b6458b953d90b6da3d53b8f6003dde1de74e1ba4c73363d28a1268617f872d07384628bf2e7fbfd1e5f96dff6d2711f71c47ee9f4f8b9a873d6efebeb27dbeb9ba4a05bd10765bca91269c1bbb8383e1f64d26b0ecc5cf8193689928d659c55beb493ba1fb1e48ec1bcf44afc9a5390ba24c389da1f0854449b9903549841d4e4749c2847f52a4183039dafb2298cc1be842b983fbf070536f1b87d5b5a728f95544639480044b728d2aaf0fe179bb610247e782168f3ceefd7df259d48cf5647a4ee5a38cf8fe26472f187e30ae3462b24b7faeb4fcf2eedfe507ec524320c2b64a07adece34c3066e81bc41bc520bc254dc191d79520cde2fd2e795e4e0d7e414dd280b25b74a5b546bb68255e5621b54d77a2cd055c4ddb1987a28ce033a14b4270887afd398bb1d03b2c4476601bb04b1da82ffcbb0a1967614976ab47a1504e9d966080a1aba25b135711db7b12ed4e4c844ea06243e51ef61ac5cdc224ac46099d46700cd5f7338a",
                StartDate = new DateTime(2007, 11, 8),
                EndDate = new DateTime(2013, 3, 22),
                Goal = 604746603,
                IsActive = true
            };

            // Act
            var serviceResult = await _challengesAppService.CreateAsync(input);

            // Assert
            var result = await _challengeRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("a57fd49877804ed8b6df3cfcbea9bf8b0e18747b4e5a400eb14c322148508327c676b379ba384446846804fd738ec4e6743c6a8dbf7d46c18e5b6c92a8685c1f299e4b1374c34984bc26e6209ecad59f2bbad8877dba4ecb8c69179d8e011d4d04a8bb146d4c4babbb4e403dba6a0038b846b259cbb44886bd97373cc9f264589234d2bb66564dad98fd73a83a03a2ebcd366a3972344d559ac6da614c6bc18925677169def148caa593a96a27e049f62eb0465ef764488f8068664abeeedf0fc2b15f5e58754631a56e8d82493eeaa106fc36e8b9a34341b473bddaa7a48aa1220b2e16510e44558a52340ea994d493afcd242cce38405baef426f2f74f2376682ecfcceb9b404480bfa68da7ed4ee32ccbb0fd1ee344449e05f81e1d450f1f51a208db017c4598b8a5a623269a1bfdaf93a31584104c14aebb213e37c2cff1d1303fae9fcd42488e1ed4acffa38852a47526f38be340dd9acf0cd1d69bbca66d1ebe03380e43778225ca183e78214ed59c5d1a894347b0b0e865c481654df948d6f8454b114a7da079ed264f7a23c5ff9a5eb1a227403d8ea7e75ec54dd8f69e8400dd755c4a0ab99bd65d339ab213e10982b749a9410eb4c59bd4291ea453eb237b8490474d6c95e93d1e5ae50a7901113ac4430b448186824ec3c89a661f1a27bd0c4efc4641b4bc5b20959067a4ae8f5e21c7d74fd58bb0e0e7f6d6ed28160d9baaab99425daa845fad88d530b67379a224569a4fadbc0a9458c9247c68bd501f4cb6594492aa1a44e7326625037f3eec91bc344b02885024b6faf8072462a11d1997274517b12dd79a71c45b8e48fee6b274444abca158fb5dd631dafea3dc4023221d45bcaf337454995d11079bfd1b02e21846ab8e03b9cc7c32571f187468783e7548d7b13bab46e022e3b123ec0c2b66b6458b953d90b6da3d53b8f6003dde1de74e1ba4c73363d28a1268617f872d07384628bf2e7fbfd1e5f96dff6d2711f71c47ee9f4f8b9a873d6efebeb27dbeb9ba4a05bd10765bca91269c1bbb8383e1f64d26b0ecc5cf8193689928d659c55beb493ba1fb1e48ec1bcf44afc9a5390ba24c389da1f0854449b9903549841d4e4749c2847f52a4183039dafb2298cc1be842b983fbf070536f1b87d5b5a728f95544639480044b728d2aaf0fe179bb610247e782168f3ceefd7df259d48cf5647a4ee5a38cf8fe26472f187e30ae3462b24b7faeb4fcf2eedfe507ec524320c2b64a07adece34c3066e81bc41bc520bc254dc191d79520cde2fd2e795e4e0d7e414dd280b25b74a5b546bb68255e5621b54d77a2cd055c4ddb1987a28ce033a14b4270887afd398bb1d03b2c4476601bb04b1da82ffcbb0a1967614976ab47a1504e9d966080a1aba25b135711db7b12ed4e4c844ea06243e51ef61ac5cdc224ac46099d46700cd5f7338a");
            result.StartDate.ShouldBe(new DateTime(2007, 11, 8));
            result.EndDate.ShouldBe(new DateTime(2013, 3, 22));
            result.Goal.ShouldBe(604746603);
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task UpdateAsync()
        {
            // Arrange
            var input = new ChallengeUpdateDto()
            {
                Name = "794bd388c77e4ffd9593430025160ab8ce91796183f44c579537c9ed2aa9f6a0a3a23315946e4f1cb910c54a40f5e23bc247dfd7f7274524b306b82be60009a2dced2560e5ec43f1ab61a1b8a7d51e512bed9f72691d46ec8b5d76ff6dc067e874cd884a594a4df8812bfc772bfb466bb278f0f42ba24c229cc8c26f1c81ebeb582fdb4deb114e5c9c59af8c7bc0f17a3dc604db377a40079d8d164a618f05ee975f865f26dd4e579939981eab9bc2afa5ab9c334a9d4e0d9ff9ebb071ae85cfe9b1d4bc84e94e5cb3c1fe8c1ed2ba0e33425a4f5f2d427caa25a206d3557ba806c8ec0155cc40b28d19e199bac902639b84957102d64a0a87e1f1b138b3eea6cdd852cb8cd944a095862901442194ec87b407ba4e2a4d8883319dba2b1020af02053bcd80524ab99400ef7312c7dc452156c41287364a2f839193d8de65d694a71d6c59cbc84a919c791d419fda2817ad7d6f44bcc849539f0b533a61d60d13b43c536f6489423eaf6007a2a36c0957f96b49eb7b154bf2b089cc8824d63ab0efdebadfae4c44a9a341299b5d722e75faa45980afc2454e9fb94c32af1a79c254cf8900365e435ba646284070ea2e7cc4f09efc9112444b9e40d447945b733d53e2f48f3706487284635863bc39ac87f64eef9c438145a49e1392f9bb739e7e3bd871960be7458a8915ca941fa67327d3a9c437fd434b14b98c34ea7e6798bcd1fbcbb7c8224ff299886225ab20e5ff814f29849adf45b88f21fcadf3b3a7484cad4235e1b6477f8f662cd3c6e7ba66fb26cacf6ae84be9927b3581af9b2da7e3ec469c00344b56a8c91bbd2a0135450b8f8f0b04ee4daf99f69a787cf97a3709c437b34a654876959d4bfa6575faf6435d3b34396f44578d534bed21da7e8232e5826d0fd04218833e294d317fa577ba8f1f2fe529469a87ffe48513442000b83ed007f04f4d3c8c45011cbc891a0c0290194f428c4d8684251a899b9aaffa3a1e578c47a04959aeeffb0104d83d3f0aa6037330ac4ee9a0d5049402bc672eae0a4cd8c18d422989cea691b774f1b1b9f507cf574d4a9885bd3300a5fc140fdc744ba54e48428493385258a2e4c9a5453e069cee7d48e18b962f9686633f512cdd4d62b4364a6bb0bf7934c9908120654430f86ec749b19d5d6c95f4837f08958c2e7eb7274c4c96a9ec62cc610f2febf60f128bd34a51aae4b4e7aac5c07265a84537b10442a4856b6fa7711b34238800922ac3fa469ebeae0accadf89fdb4e3c55503f7844b3bafa28ef969a7b42df2f212c289044669522dc2483014a2494f1d405f0c842d0903341253731a8843fd032d30892405a9467af728c460b9aae9d7f8c797846c684b9085e1e6583d13de2f72c7abd411abc0fc2911e9e730aaf738f931f3d41b7b20af37bb19c794e2ce87f5b6c22412b890ff63c6407e3f9",
                StartDate = new DateTime(2012, 10, 25),
                EndDate = new DateTime(2018, 8, 21),
                Goal = 1661525841,
                IsActive = true
            };

            // Act
            var serviceResult = await _challengesAppService.UpdateAsync(Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f"), input);

            // Assert
            var result = await _challengeRepository.FindAsync(c => c.Id == serviceResult.Id);

            result.ShouldNotBe(null);
            result.Name.ShouldBe("794bd388c77e4ffd9593430025160ab8ce91796183f44c579537c9ed2aa9f6a0a3a23315946e4f1cb910c54a40f5e23bc247dfd7f7274524b306b82be60009a2dced2560e5ec43f1ab61a1b8a7d51e512bed9f72691d46ec8b5d76ff6dc067e874cd884a594a4df8812bfc772bfb466bb278f0f42ba24c229cc8c26f1c81ebeb582fdb4deb114e5c9c59af8c7bc0f17a3dc604db377a40079d8d164a618f05ee975f865f26dd4e579939981eab9bc2afa5ab9c334a9d4e0d9ff9ebb071ae85cfe9b1d4bc84e94e5cb3c1fe8c1ed2ba0e33425a4f5f2d427caa25a206d3557ba806c8ec0155cc40b28d19e199bac902639b84957102d64a0a87e1f1b138b3eea6cdd852cb8cd944a095862901442194ec87b407ba4e2a4d8883319dba2b1020af02053bcd80524ab99400ef7312c7dc452156c41287364a2f839193d8de65d694a71d6c59cbc84a919c791d419fda2817ad7d6f44bcc849539f0b533a61d60d13b43c536f6489423eaf6007a2a36c0957f96b49eb7b154bf2b089cc8824d63ab0efdebadfae4c44a9a341299b5d722e75faa45980afc2454e9fb94c32af1a79c254cf8900365e435ba646284070ea2e7cc4f09efc9112444b9e40d447945b733d53e2f48f3706487284635863bc39ac87f64eef9c438145a49e1392f9bb739e7e3bd871960be7458a8915ca941fa67327d3a9c437fd434b14b98c34ea7e6798bcd1fbcbb7c8224ff299886225ab20e5ff814f29849adf45b88f21fcadf3b3a7484cad4235e1b6477f8f662cd3c6e7ba66fb26cacf6ae84be9927b3581af9b2da7e3ec469c00344b56a8c91bbd2a0135450b8f8f0b04ee4daf99f69a787cf97a3709c437b34a654876959d4bfa6575faf6435d3b34396f44578d534bed21da7e8232e5826d0fd04218833e294d317fa577ba8f1f2fe529469a87ffe48513442000b83ed007f04f4d3c8c45011cbc891a0c0290194f428c4d8684251a899b9aaffa3a1e578c47a04959aeeffb0104d83d3f0aa6037330ac4ee9a0d5049402bc672eae0a4cd8c18d422989cea691b774f1b1b9f507cf574d4a9885bd3300a5fc140fdc744ba54e48428493385258a2e4c9a5453e069cee7d48e18b962f9686633f512cdd4d62b4364a6bb0bf7934c9908120654430f86ec749b19d5d6c95f4837f08958c2e7eb7274c4c96a9ec62cc610f2febf60f128bd34a51aae4b4e7aac5c07265a84537b10442a4856b6fa7711b34238800922ac3fa469ebeae0accadf89fdb4e3c55503f7844b3bafa28ef969a7b42df2f212c289044669522dc2483014a2494f1d405f0c842d0903341253731a8843fd032d30892405a9467af728c460b9aae9d7f8c797846c684b9085e1e6583d13de2f72c7abd411abc0fc2911e9e730aaf738f931f3d41b7b20af37bb19c794e2ce87f5b6c22412b890ff63c6407e3f9");
            result.StartDate.ShouldBe(new DateTime(2012, 10, 25));
            result.EndDate.ShouldBe(new DateTime(2018, 8, 21));
            result.Goal.ShouldBe(1661525841);
            result.IsActive.ShouldBe(true);
        }

        [Fact]
        public async Task DeleteAsync()
        {
            // Act
            await _challengesAppService.DeleteAsync(Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f"));

            // Assert
            var result = await _challengeRepository.FindAsync(c => c.Id == Guid.Parse("4882ddb4-f720-4b12-96dd-2d805da5c80f"));

            result.ShouldBeNull();
        }
    }
}
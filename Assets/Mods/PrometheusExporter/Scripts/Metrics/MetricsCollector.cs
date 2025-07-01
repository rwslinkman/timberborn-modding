using Timberborn.Goods;
using Timberborn.ScienceSystem;
using Timberborn.SingletonSystem;
using Timberborn.ResourceCountingSystem;
using Timberborn.Beavers;
using Timberborn.Bots;

namespace PrometheusExporter.Metrics
{
    class MetricsCollector : ILoadableSingleton
    {
        private readonly EventBus eventBus;
        private readonly IGoodService timberbornGoodsService;
        private readonly ScienceService timberbornScienceService;
        private readonly ResourceCountingService timberbornResourceCountingService;
        private readonly BeaverPopulation timberbornBeaverPopulation;
        private readonly BotPopulation timberbornBotPopulation;
        private int samplesCollected;

        public MetricsCollector(
            EventBus bus,
            IGoodService goodService,
            ScienceService scienceService,
            ResourceCountingService resourceCountingService,
            BeaverPopulation beaverPopulation,
            BotPopulation botPopulation
        )
        {
            this.eventBus = bus;
            this.timberbornGoodsService = goodService;
            this.timberbornScienceService = scienceService;
            this.timberbornResourceCountingService = resourceCountingService;
            this.timberbornBeaverPopulation = beaverPopulation;
            this.timberbornBotPopulation = botPopulation;
            this.samplesCollected = 0;
        }

        public void Load()
        {
            eventBus.Register(this);
        }

        [OnEvent]
        public void CollectMetrics(CollectMetricsCommand cmd)
        {
            samplesCollected++;

            var metrics = new MetricsCollectedEvent();
            metrics.SampleNumber = samplesCollected;

            // Collect data from game, store in event class
            metrics.TotalBeaverCount = this.timberbornBeaverPopulation.NumberOfBeavers;
            metrics.AdultBeaverCount = this.timberbornBeaverPopulation.NumberOfAdults;
            metrics.ChildBeaverCount = this.timberbornBeaverPopulation.NumberOfChildren;
            metrics.TotalBotCount = this.timberbornBotPopulation.NumberOfBots;
            metrics.SciencePoints = this.timberbornScienceService.SciencePoints;
            foreach (var goodId in timberbornGoodsService.Goods)
            {
                var resourceCount = timberbornResourceCountingService.GetGlobalResourceCount(goodId);
                var resourceCapacity = CalculateTotalCapacity(resourceCount);
                metrics.GoodAmounts.Add(goodId, resourceCount.TotalStock);
                metrics.GoodCapacities.Add(goodId, resourceCapacity);
            }
            eventBus.Post(metrics);
        }
        
        private static int CalculateTotalCapacity(ResourceCount resourceCount) {
            if (resourceCount.FillRate == 0) {
                return resourceCount.InputOutputCapacity;
            }
            return (int) (resourceCount.TotalStock / resourceCount.FillRate);
        }
    }
}
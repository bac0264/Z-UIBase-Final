public class WatchAdsListenerData : BaseListenerData
{
    public WatchAdsListenerData(object obj) : base(obj)
    {
        subjectType = SubjectType.WatchAds;
        PublisherService.ResgisterModule(subjectType, this);
    }
}

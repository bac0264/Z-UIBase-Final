public class CampaignListenerData : BaseListenerData
{
    public CampaignListenerData(object obj) : base(obj)
    {
        subjectType = SubjectType.PlayCampaign;
        PublisherService.ResgisterModule(subjectType, this);
    }
    
    // public CampaignListenerData(SubjectType subjectType, int number) : base(subjectType, number)
    // {
    //     this.subjectType = subjectType;
    //     this.number = number;
    // }
    
}

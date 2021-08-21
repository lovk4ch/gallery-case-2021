using System;

namespace gallery_case_2021
{
    public class PlayerData
    {
        private DateTimeOffset billingDate;
        private string mediaItemName;
        private int duration;
        private string mac;
        private long? addedOnTick;

        public DateTimeOffset BillingDate { get => billingDate; set => billingDate = value; }
        public string MediaItemName { get => mediaItemName; set => mediaItemName = value; }
        public int Duration { get => duration; set => duration = value; }
        public string Mac { get => mac; set => mac = value; }
        public long? AddedOnTick { get => addedOnTick; set => addedOnTick = value; }

        public PlayerData(DateTimeOffset billingDate, string mediaItemName, int duration, string mac, long? addedOnTick)
        {
            this.billingDate = billingDate;
            this.mediaItemName = mediaItemName;
            this.duration = duration;
            this.mac = mac;
            this.addedOnTick = addedOnTick;
        }
    }
}
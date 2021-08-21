using System;

namespace gallery_case_2021
{
    public class PlayerLog
    {
        private ulong oidColumn;
        private DateTimeOffset billingDate;
        private string mediaItemName;
        private int duration;

        public ulong OidColumn { get => oidColumn; set => oidColumn = value; }
        public DateTimeOffset BillingDate { get => billingDate; set => billingDate = value; }
        public string MediaItemName { get => mediaItemName; set => mediaItemName = value; }
        public int Duration { get => duration; set => duration = value; }

        public PlayerLog(ulong oidColumn, DateTimeOffset billingDate, string mediaItemName, int duration)
        {
            this.OidColumn = oidColumn;
            this.BillingDate = billingDate;
            this.MediaItemName = mediaItemName;
            this.Duration = duration;
        }
    }
}
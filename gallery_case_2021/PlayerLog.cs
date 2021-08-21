using System;

namespace gallery_case_2021
{
    public class PlayerLog
    {
        private ulong oidColumn;
        private DateTimeOffset billingDate;
        private string mediaItemName;
        private int duration;
        private long dateTimeInTick;
        private long dateTimeOutTick;

        public ulong OidColumn { get => oidColumn; set => oidColumn = value; }
        public DateTimeOffset BillingDate { get => billingDate; set => billingDate = value; }
        public string MediaItemName { get => mediaItemName; set => mediaItemName = value; }
        public int Duration { get => duration; set => duration = value; }
        public long DateTimeInTick { get => dateTimeInTick; set => dateTimeInTick = value; }
        public long DateTimeOutTick { get => dateTimeOutTick; set => dateTimeOutTick = value; }

        public PlayerLog(ulong oidColumn, DateTimeOffset billingDate, string mediaItemName, int duration, long dateTimeInTick, long dateTimeOutTick)
        {
            this.OidColumn = oidColumn;
            this.BillingDate = billingDate;
            this.MediaItemName = mediaItemName;
            this.Duration = duration;
            this.DateTimeInTick = dateTimeInTick;
            this.DateTimeOutTick = dateTimeOutTick;
        }
    }
}
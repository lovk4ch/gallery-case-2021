using System;

namespace gallery_case_2021
{
    public class PlayerLog
    {
        public ulong oidColumn;
        public DateTime billingDate;
        public string mediaItemName;
        public int duration;
        public long dateTimeInTick;
        public long dateTimeOutTick;

        public PlayerLog(ulong oidColumn, string mediaItemName, int duration, long dateTimeInTick, long dateTimeOutTick)
        {
            this.oidColumn = oidColumn;
            this.billingDate = DateTime.FromFileTimeUtc(dateTimeInTick);
            this.mediaItemName = mediaItemName;
            this.duration = duration;
            this.dateTimeInTick = dateTimeInTick;
            this.dateTimeOutTick = dateTimeOutTick;
        }
    }
}
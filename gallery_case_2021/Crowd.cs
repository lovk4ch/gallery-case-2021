namespace gallery_case_2021
{
    public class Crowd
    {
        public ulong oidColumn;
        public string mac;
        public long? addedOnTick;
        public ulong oidFrame;

        public Crowd(ulong oidColumn, string mac, long? addedOnTick, ulong oidFrame)
        {
            this.oidColumn = oidColumn;
            this.mac = mac;
            this.addedOnTick = addedOnTick;
            this.oidFrame = oidFrame;
        }
    }
}

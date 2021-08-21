namespace gallery_case_2021
{
    public class Crowd
    {
        private ulong oidColumn;
        private string mac;
        private long? addedOnTick;
        private ulong oidFrame;

        public ulong OidColumn { get => oidColumn; set => oidColumn = value; }
        public string Mac { get => mac; set => mac = value; }
        public long? AddedOnTick { get => addedOnTick; set => addedOnTick = value; }
        public ulong OidFrame { get => oidFrame; set => oidFrame = value; }

        public Crowd(ulong oidColumn, string mac, long? addedOnTick, ulong oidFrame)
        {
            this.OidColumn = oidColumn;
            this.Mac = mac;
            this.AddedOnTick = addedOnTick;
            this.OidFrame = oidFrame;
        }
    }
}

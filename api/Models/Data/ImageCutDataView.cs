using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Data
{
    public class ImageCutDataView
    {
        public string Src { get; set; }
        public int ImageWidth { get; set; }
        public int ImageHeight { get; set; }
        public int CutTop { get; set; }
        public int CutLeft { get; set; }
        public int DropWidth { get; set; }
        public int DropHeight { get; set; }
        public int Zoom { get; set; }
    }
}

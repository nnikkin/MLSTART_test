using MallenomTest.ViewModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace MallenomTest.Models
{
    // Модель для изображения
    public class Image: ViewModelBase
    {
        private byte[] imagefile;
        private string name;
        private string path;

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public static int count = 0;
        public string Name {
            get => name; 
            set
            {
                name = value;
                base.OnPropertyChanged();
            }
        }
        public string Path {
            get => path;
            set
            {
                path = value;
                base.OnPropertyChanged();
            }
        }

        public byte[] ImageFile { 
            get => imagefile;
            set
            {
                imagefile = value;
                base.OnPropertyChanged(nameof(ImageSource));
            } 
        }

        public Image(string name, string path)
        {
            Name = name;
            Path = path;
            ImageFile = LoadImage(path);
        }

        public byte[] LoadImage(string filepath)
        {
            return File.ReadAllBytes(filepath);
        }
    }
}
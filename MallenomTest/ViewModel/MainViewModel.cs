using MallenomTest.Abstractions;
using MallenomTest.Commands;
using MallenomTest.Data;
using MallenomTest.Models;
using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows;
using System.Windows.Input;

namespace MallenomTest.ViewModel
{
    // ViewModel для главного окна
    class MainViewModel : ViewModelBase
    {
        IDialogService dialogService;

        // Коллекция изображений
        private readonly ObservableCollection<Image> ImageFiles;
        public ObservableCollection<Image> Images { get; set; }

        // Выбранное изображение
        private Image selectedImage;
        public Image? SelectedImage
        {
            get
            {
                return selectedImage;
            }
            set
            {
                selectedImage = value;
                OnPropertyChanged("SelectedImage");
                updateCommand?.RaiseCanExecuteChanged();
                deleteCommand?.RaiseCanExecuteChanged();
            }
        }

        // Команда добавления изображения
        private RelayCommand addCommand;

        public ICommand AddCommand => addCommand ??
                (addCommand = new RelayCommand(obj =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Title = "Добавить изображение",
                        Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        var path = dialog.FileName;
                        var name = System.IO.Path.GetFileName(path);
                        var imageFile = new Image(name, path)
                        {
                            Id = Images.Count + 1,
                            ImageFile = File.ReadAllBytes(path)
                        };
                        Images.Add(imageFile);

                        using (var context = new AppDbContext())
                        {
                            context.Images.Add(imageFile);
                            context.SaveChanges();
                        }
                    }
                }));

        // Команда обновления изображения
        private RelayCommand updateCommand;
        public ICommand UpdateCommand => updateCommand ??
                (updateCommand = new RelayCommand(obj =>
                {
                    var dialog = new OpenFileDialog
                    {
                        Title = "Изменить изображение",
                        Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*"
                    };
                    if (dialog.ShowDialog() == true)
                    {
                        var path = dialog.FileName;
                        var name = System.IO.Path.GetFileName(path);
                        if (SelectedImage != null)
                        {
                            SelectedImage.Name = name;
                            SelectedImage.Path = path;
                            SelectedImage.ImageFile = File.ReadAllBytes(path);

                            using (var context = new AppDbContext())
                            {
                                context.Images.Update(SelectedImage);
                                context.SaveChanges();
                            }

                            SelectedImage = null;
                            OnPropertyChanged("Images");
                        }
                    }
                }, obj => SelectedImage != null));

        // Команда удаления изображения
        private RelayCommand deleteCommand;
        public ICommand DeleteCommand => deleteCommand ??
                (deleteCommand = new RelayCommand(obj =>
                {
                    if (SelectedImage != null)
                    {
                        string name = SelectedImage.Name;
                        using (var context = new AppDbContext())
                        {
                            Image.count--;
                            context.Images.Remove(SelectedImage);
                            context.SaveChanges();
                        }

                        Images.Remove(SelectedImage);
                        SelectedImage = null;
                        OnPropertyChanged("Images");

                        MessageBox.Show("Изображение " + name + " удалено", "", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }, obj => SelectedImage != null));

        public MainViewModel(IDialogService dialogService)
        {
            this.dialogService = dialogService;

            Images = [];

            using (var context = new AppDbContext())
            {
                var imageFiles = context.Images.ToList();
                foreach (var imageFile in imageFiles)
                {
                    Images.Add(imageFile);
                }
            }
        }
    }
}

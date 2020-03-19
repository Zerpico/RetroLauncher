using RetroLauncher.Client.Controls;
using RetroLauncher.ServiceTools.Emuplace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Key = System.Windows.Input.Key;

namespace RetroLauncher.Client.Views
{
    /// <summary>
    /// Логика взаимодействия для SettingEmulatorView.xaml
    /// </summary>
    public partial class SettingEmulatorDialog : UserControl
    {
        ConfigBaseSettings config;
        public SettingEmulatorDialog()
        {
            InitializeComponent();
            config = new ConfigBaseSettings();

            GenerateUI();
        }

        private void GenerateUI()
        {
            //перебираем все свойства класса настроек
            foreach (var prop in typeof(ConfigBaseSettings).GetProperties())
            {
                int itemIndex = 0;

                //индекс TabControl
                if (prop.Name.StartsWith("nes"))
                    itemIndex = 1;
                else if (prop.Name.StartsWith("sms"))
                    itemIndex = 2;
                else if (prop.Name.StartsWith("snes"))
                    itemIndex = 3;
                else if (prop.Name.StartsWith("md"))
                    itemIndex = 4;
                else if (prop.Name.StartsWith("gb"))
                    itemIndex = 5;

                //описание свойства из атрибута
                var description = prop.CustomAttributes.Where(d => d.AttributeType.Name == "DescriptionProperty").First();

                var values = prop.GetValue(config);
                

                //панель управления
                if (prop.Name.Contains("__input__"))
                {
                    bool isContain = false;
                    int indexGroup = 0;

                    var count = (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).Children.Count;

                    for (int i = 0; i < count; i++)
                        if ((((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(GroupBox))
                        { 
                            isContain = true; 
                            indexGroup = i; 
                        }


                    if (!isContain)
                    {
                        var newGroup = new GroupBox() { Header = "Управление" };
                        newGroup.Margin = new Thickness(3, 10, 3, 3);
                        var newPanelContain = new StackPanel() { Margin = new Thickness(3, 3, 3, 3) };
/*
                        switch (itemIndex)
                        {
                            case 1:
                                newPanelContain.Children.Add(new Image() { Width = 128, Source = new BitmapImage(new Uri("pack://application:,,,/icon/gamepad_nes.png")) });
                                break;
                            case 2:
                                newPanelContain.Children.Add(new Image() { Width = 128, Source = new BitmapImage(new Uri("pack://application:,,,/icon/gamepad_sms.png")) });
                                break;
                            case 3:
                                newPanelContain.Children.Add(new Image() { Width = 128, Source = new BitmapImage(new Uri("pack://application:,,,/icon/gamepad_snes.png")) });
                                break;
                            case 4:
                                newPanelContain.Children.Add(new Image() { Width = 128, Source = new BitmapImage(new Uri("pack://application:,,,/icon/gamepad_md.png")) });
                                break;
                            case 5:
                                newPanelContain.Children.Add(new Image() { Width = 128, Source = new BitmapImage(new Uri("pack://application:,,,/icon/gamepad_gb.png")) });
                                break;

                        }
*/
                        newGroup.Content = newPanelContain;
                        (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                             Children.Add(newGroup);


                        isContain = true;
                        indexGroup = (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).Children.Count - 1;
                    }

                    //поле для назначения клавищ
                    string buttonValue = "";
                    if (values != null)
                    {
                        if (values.ToString().Contains("keyboard"))
                        {
                            var key = System.Windows.Input.KeyInterop.KeyFromVirtualKey(Keyassign.SdlKeyToVKey(int.Parse(values.ToString().Split(' ')[2])));
                            buttonValue = Enum.Parse(typeof(System.Windows.Input.Key), key.ToString(), true).ToString();
                        }
                    }

                    var newTextbox = new TextBox()
                    {
                        Text = buttonValue,
                        //MaxWidth = 400,
                        Name = prop.Name,
                        AcceptsReturn = false,
                        AcceptsTab = true,
                        IsReadOnly = true
                    };
                    newTextbox.GotFocus += (e, o) =>
                    {
                        var brush = new SolidColorBrush(Color.FromArgb(105, 91, 64, 104));
                        newTextbox.Background = brush;
                    };
                    newTextbox.LostFocus += (e, o) =>
                    {
                        var style = (Brush)FindResource("WindowBrush");
                        newTextbox.Background = style;
                    };
                    newTextbox.PreviewKeyDown += (e, o) =>
                    {
                        if (o.Key == Key.Escape)
                            newTextbox.Text = null;
                        else newTextbox.Text = o.Key.ToString().Trim();
                    };


                    (((((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).Children[indexGroup] as GroupBox).Content as Panel).
                           Children.Add(new TextBlock()
                           {
                               Margin = new Thickness(0, 3, 0, 0),
                               Text = description.ConstructorArguments.First().Value.ToString(),
                               //MaxWidth = 400,
                               TextWrapping = TextWrapping.Wrap
                           });

                    (((((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).Children[indexGroup] as GroupBox).Content as Panel).
                            Children.Add(newTextbox);

                }

                else if (values.GetType() == typeof(bool))
                {
                    var text = new TextBlock()
                    {
                        Text = description.ConstructorArguments.First().Value.ToString(),
                        TextWrapping = TextWrapping.Wrap
                    };

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                          Children.Add(new CheckBox()
                          {
                              IsChecked = (bool)prop.GetValue(config),
                              Name = prop.Name,
                              //MaxWidth = 400,                              
                              Content = text,//description.ConstructorArguments.First().Value.ToString(),
                              Margin = new Thickness(0, 3, 0, 3)
                          });
                }
                else if (values.GetType() == typeof(double))
                {

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                      Children.Add(new TextBlock()
                      {
                          Margin = new Thickness(0, 3, 0, 0),
                          Text = description.ConstructorArguments.First().Value.ToString(),
                          MaxWidth = 400,
                          TextWrapping = TextWrapping.Wrap
                      });

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                       /*  Children.Add(new TextBox()
                         {
                             Margin = new Thickness(0, 0, 0, 3),
                             Text = prop.GetValue(config).ToString(),
                             //Increment = 0.1,
                             Name = prop.Name,
                             MaxWidth = 400
                         });*/
                       /* Children.Add(new DoubleUpDown()
                        {
                            Margin = new Thickness(0, 0, 0, 3),
                            Value = (double)prop.GetValue(config),
                            Increment = 0.1,
                            Name = prop.Name,
                            Width = 400,
                        });*/
                       Children.Add(new NumericUpDown()
                       {
                           Margin = new Thickness(0, 0, 0, 3),
                           Value = (double)prop.GetValue(config),
                           Name = prop.Name,
                           Maximum = 10,
                           Minimum = 0,
                           Interval = 0.1,
                           StringFormat = "F1"

                           // Width = 400,
                       });

                }
                else if (values.GetType() == typeof(int))
                {

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                       Children.Add(new TextBlock()
                       {
                           Margin = new Thickness(0, 3, 0, 0),
                           Text = description.ConstructorArguments.First().Value.ToString(),
                           //MaxWidth = 400,
                           TextWrapping = TextWrapping.Wrap
                       });

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                       /* Children.Add(new TextBox()
                        {
                            Margin = new Thickness(0, 0, 0, 3),
                            Text = prop.GetValue(config).ToString(),
                            //Increment = 0.1,
                            Name = prop.Name,
                            //MaxWidth = 400
                        });*/
                    Children.Add(new NumericUpDown()
                    {
                        Margin = new Thickness(0, 0, 0, 3),
                        Value = (int)prop.GetValue(config),
                        Name = prop.Name,
                        Maximum = 100000,                        
                        Interval = 1,
                        StringFormat="F0"
                        
                       // Width = 400,
                    });
                }
                else if (values.GetType().IsEnum)
                {
                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                       Children.Add(new TextBlock()
                       {
                           Margin = new Thickness(0, 3, 0, 0),
                           Text = description.ConstructorArguments.First().Value.ToString(),
                           //MaxWidth = 400,
                           TextWrapping = TextWrapping.Wrap
                       });

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                      Children.Add(new ComboBox()
                      {

                          Margin = new Thickness(0, 0, 0, 3),
                          ItemsSource = Enum.GetValues(values.GetType()),
                          Text = prop.GetValue(config).ToString(),
                          Name = prop.Name,
                          //MaxWidth = 400,
                          Background = Brushes.White
                      });
                }
                else if (values.GetType() == typeof(string))
                {
                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                      Children.Add(new TextBlock()
                      {
                          Margin = new Thickness(0, 3, 0, 0),
                          Text = description.ConstructorArguments.First().Value.ToString(),
                          //MaxWidth = 400,
                          TextWrapping = TextWrapping.Wrap
                      });

                    (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                      Children.Add(new TextBox()
                      {
                          Margin = new Thickness(0, 0, 0, 3),
                          Text = prop.GetValue(config).ToString(),
                          //MaxWidth = 400,
                          Name = prop.Name,
                      });

                    /* var newPanel = new StackPanel();
                     newPanel.Margin = new Thickness(3);
                     newPanel.Children.Add(new TextBlock()
                     {
                         Text = description.ConstructorArguments.First().Value.ToString(),
                         MaxWidth = 400,
                         TextWrapping = TextWrapping.Wrap
                     });
                     newPanel.Children.Add(new TextBox()
                     {
                         Text = prop.GetValue(config).ToString(),
                         MaxWidth = 400,
                         Name = prop.Name,
                     });

                     (((mainTabcontrol.Items[itemIndex] as ContentControl).Content as ContentControl).Content as Panel).
                        Children.Add(newPanel);*/
                }

            }
        
        }



        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            var propSettings = typeof(ConfigBaseSettings).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance);

            foreach (TabItem tabitem in mainTabcontrol.Items)
            {
                if ((tabitem.Content as ContentControl).Content != null)
                    for (int i = 0; i < ((tabitem.Content as ContentControl).Content as Panel).Children.Count; i++)
                    {
                        if (((tabitem.Content as ContentControl).Content as Panel).Children[i] is Control)
                        {
                            if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(CheckBox))
                            {
                                propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as CheckBox).IsChecked);
                            }
                            else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(NumericUpDown))
                            {
                                var numControl = (((tabitem.Content as ContentControl).Content as Panel).Children[i] as NumericUpDown);

                                if (numControl.Interval == 1)
                                {
                                    propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, new Nullable<int>((int)numControl.Value));
                                }
                                else if (numControl.Interval == 0.1)
                                {
                                    propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, new Nullable<double>((double)numControl.Value));
                                }

                               // propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                               //     SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as NumericUpDown).Value);
                            }
                            /*else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(DoubleUpDown))
                            {
                                propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as DoubleUpDown).Value);
                            }
                            else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(IntegerUpDown))
                            {
                                propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as IntegerUpDown).Value);
                            }*/
                            else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(ComboBox))
                            {
                                var enumType = propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First();
                                if (enumType.PropertyType.IsEnum)
                                {
                                    var enumValue = Enum.Parse(enumType.PropertyType, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as ComboBox).Text);
                                    enumType.SetValue(config, enumValue);
                                }
                            }
                            else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(TextBox))
                            {
                                propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as TextBox).Text);
                            }
                            else if (((tabitem.Content as ContentControl).Content as Panel).Children[i].GetType() == typeof(GroupBox))
                            {
                                var groupPanel = (((tabitem.Content as ContentControl).Content as Panel).Children[i] as ContentControl).Content as Panel;

                                for (int j = 0; j < groupPanel.Children.Count; j++)
                                {
                                    if (groupPanel.Children[j].GetType() == typeof(TextBox))
                                    {
                                        propSettings.Where(d => d.Name == (groupPanel.Children[j] as Control).Name).First().
                                            SetValue(config, (groupPanel.Children[j] as TextBox).Text);
                                    }
                                }

                                /*propSettings.Where(d => d.Name == (((tabitem.Content as ContentControl).Content as Panel).Children[i] as Control).Name).First().
                                    SetValue(config, (((tabitem.Content as ContentControl).Content as Panel).Children[i] as TextBox).Text);*/
                            }

                        }
                    }
            }

            config.Save();
        }

        private void ButtonCancel_Click(object sender, RoutedEventArgs e)
        {
           // this.Close();
        }


    }
}

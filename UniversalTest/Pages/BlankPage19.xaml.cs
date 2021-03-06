﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SQLite.Net;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace UniversalTest.Pages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class BlankPage19 : Page
    {
        SQLiteConnectionPool _connectionPool = new SQLiteConnectionPool(new SQLitePlatformWinRT());
        public BlankPage19()
        {
            this.InitializeComponent();
            Loaded += BlankPage19_Loaded;
        }

        private void BlankPage19_Loaded(object sender, RoutedEventArgs e)
        {
            using (var c = GetConnection())
            {
                c.CreateTable<TestTable>();
            }
        }

        private SQLiteConnection GetConnection()
        {
            var connectionstring = ApplicationData.Current.LocalFolder.Path + "\\test.db";
            var connection = new SQLiteConnection(new SQLitePlatformWinRT(), connectionstring);

            // var connection = _connectionPool.GetConnection(new SQLiteConnectionString(connectionstring, true));
            return connection;
        }

        private void Write_click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("write start");
            Task.Run(() =>
            {
                try
                {
                    using (var c = GetConnection())
                    {
                        for (int i = 0; i < 500; i++)
                        {
                            var t = new TestTable()
                            {
                                T = i.ToString()
                            };
                            c.Insert(t);
                            Debug.WriteLine("write " + i + " ## ");
                        }
                        Debug.WriteLine("------------------------------write finished");
                    }
                }
                catch (Exception exception)
                {
                    Debug.Assert(false);
                }
            });
        }

        private void Read_click(object sender, RoutedEventArgs e)
        {
            Debug.WriteLine("read start");
            Task.Run(() =>
            {
                try
                {
                    using (var c = GetConnection())
                    {
                        for (int i = 0; i < 1000; i++)
                        {
                            var t = c.Query<TestTable>("select * from TestTable");
                            Debug.WriteLine("read " + i + " count: " + t.Count + " ## ");
                        }
                        Debug.WriteLine("------------------------read finished");
                    }
                }
                catch (Exception exception)
                {
                    Debug.Assert(false);
                }
            });
        }



    }

    class TestTable
    {
        public string T { get; set; }
    }
}

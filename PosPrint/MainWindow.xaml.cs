﻿using PosPrint.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Printing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace PosPrint
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var businessInfo = Model.Business.Businesses.FirstOrDefault();
            Title = "LOGS MODULE MONITORING";

            txtPrice1.Text = "121";
            txtDescription.Text = "YEllow cab";
            txtQuantity.Text = "1";
            txtTendered.Text = "2323";
            txtBalance.Text = "13232";
            txtTax.Text = "232";
            txtDiscount.Text = "454545";
            lbTotal.Visibility = Visibility.Hidden;
            dgCart.Visibility = Visibility.Hidden;
          txtBalance.Visibility = Visibility.Hidden;
         txtSubtotal.Visibility = Visibility.Hidden;
            txtTax.Visibility = Visibility.Hidden;
        txtTendered.Visibility = Visibility.Hidden;
          txtDiscount.Visibility = Visibility.Hidden;
        }
        decimal payable = 0;//this is the total amount to be payed
        decimal tendered = 0;// this is the ammount presented to the cashier
        decimal balance = 0;// this is the change to be offerd to customer

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            Order.orders.Add(new Order
            {
                ProductName = txtProduct.Text,
                Quantity = Decimal.Parse(txtQuantity.Text),
                Price = Decimal.Parse(txtPrice1.Text),
                ProductDescribtion = txtDescription.Text,

            });

            dgCart.ItemsSource = Order.orders;
            dgCart.Items.Refresh();

            var total = Math.Round(Order.orders.Sum(x => x.Amount), 2);
            var tax = total * Order.Tax;
            var discount = total * Order.Discount;
            var subtotal = (total + tax) - discount;
            payable = Math.Round(subtotal, 2);



            txtSubtotal.Text = total.ToString();
            txtTax.Text = tax.ToString();
            txtDiscount.Text = discount.ToString();
            lbTotal.Content = payable;

            txtQuantity.Clear();
            txtDescription.Clear();
            txtPrice1.Clear();
            txtProduct.Clear();

            //dgCart.DataContext = cart;

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(txtProduct.Text))
            {
                //txtTendered.Text = Math.Round(Decimal.Parse(txtTendered.Text), 2).ToString();
                MessageBox.Show("Please Select in Or Out !");
                return;

            }

            else
            {
                Print();
                txtProduct.Text = "";
                //var main = new BusinessSetup();
                //main.Show();

                //this.Close();
            }
        }


        public void Print()
        {
            var doc = new PrintDocument();
            var paperSize = new PaperSize("Custom", 520, 220);
            doc.DefaultPageSettings.PaperSize = paperSize;
            //doc.DefaultPageSettings.PaperSize.Kind = PaperKind.Custom;
            //doc.DefaultPageSettings.PaperSize.Height = 820;
            //doc.DefaultPageSettings.PaperSize.Width = 520;
            doc.PrintPage += new PrintPageEventHandler(ProvideContent);
            //var pd = new PrintDialog();
            //pd.ShowDialog();
            doc.Print();
        }

        public void ProvideContent(object sender, PrintPageEventArgs e)
        {

            tendered = Math.Round(Decimal.Parse(txtTendered.Text));//

            decimal total = decimal.Parse(lbTotal.Content.ToString());
            var receiptItems = Order.orders;
            const int FIRST_COL_PAD = 5;
            const int SECOND_COL_PAD = 7;
            const int THIRD_COL_PAD = 8;


            var sb = new StringBuilder();
            var business = new Business();
            //replace with item.Branch
            sb.AppendLine((business.BusinessName));
            sb.AppendLine(" ");
            sb.AppendLine(("STORE BRANCH: FRESH OPTIONS"));
            sb.AppendLine(" ");
            sb.AppendLine(("STORE MANAGER:       ").PadLeft(20));
            sb.AppendLine(("STORE MANAGER SIGNATORY.:  "));
            //sb.AppendLine(("TEL: 09650695254 "));
            sb.AppendLine(" ");
            sb.Append(("DATE").PadRight(8));
            sb.AppendLine(": " + DateTime.Now);
            sb.Append(("STATUS:'"+txtProduct.Text+"'").PadLeft(15));
            sb.AppendLine((": "));
            sb.AppendLine(" ");
            //sb.AppendLine("=".PadRight(35,'='));
            //sb.Append(("ITEM").PadLeft(15));
            //sb.Append(("QTY").PadLeft(FIRST_COL_PAD));
            //sb.Append(("PRICE").PadLeft(SECOND_COL_PAD));
            //sb.AppendLine(("GH?").PadLeft(THIRD_COL_PAD));
            //sb.AppendLine("-".PadRight(60, '-'));


            foreach (var item in receiptItems)
            {

                string pd = "";
                if (item.ProductName.Length > 15)
                {

                    pd = item.ProductName.Substring(0, 15);


                }
                else
                {

                    pd = item.ProductName;
                }
                sb.Append(pd.PadLeft(15));
                //sb.Append(item.Quantity.ToString().PadLeft(FIRST_COL_PAD));
                //sb.Append((item.Price).ToString("F", CultureInfo.InvariantCulture).PadLeft(SECOND_COL_PAD));
                //sb.AppendLine((string.Format("{0:0.00}", item.Amount)).ToString().PadLeft(THIRD_COL_PAD));
            }
            //sb.AppendLine("-".PadRight(60, '-'));
            //sb.Append("Sub Total:".PadLeft(13 + FIRST_COL_PAD + SECOND_COL_PAD));
            //sb.AppendLine(string.Format("{0:0.00}", total));
            //sb.AppendLine("VAT @ 17.50%: ".PadLeft(13 + FIRST_COL_PAD + SECOND_COL_PAD));
            //sb.AppendLine("=".PadRight(50, '='));
            //sb.AppendLine("Bill Total:".PadLeft(15 + FIRST_COL_PAD + SECOND_COL_PAD));

            //sb.AppendLine("=".PadRight(50, '='));


            var printText = new PrintText(sb.ToString(), new Font(System.Drawing.FontFamily.GenericMonospace, 9, System.Drawing.FontStyle.Bold));
            Graphics graphics = e.Graphics;
            int startX = 0;
            int startY = 0;
            int Offset = 20;

            graphics.DrawString(printText.Text, new Font(System.Drawing.FontFamily.GenericMonospace, 9, System.Drawing.FontStyle.Bold),
                                new SolidBrush(System.Drawing.Color.Black), startX, startY + Offset);
            Offset = Offset + 20;
        }

        private void txtTendered_LostFocus(object sender, RoutedEventArgs e)
        {
            if (txtBalance.Text != null)
            {
                //txtTendered.Text = Math.Round(Decimal.Parse(txtTendered.Text), 2).ToString();
                tendered = Math.Round(Decimal.Parse(txtTendered.Text));//
                txtBalance.Text = (tendered - payable).ToString();//

            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            txtProduct.Text = "";
            txtProduct.Text = "IN ARRIVAL";
        }

        private void RadioButton_Checked_1(object sender, RoutedEventArgs e)
        {
            dgCart.Items.Refresh();
            txtProduct.Text = "";
            txtProduct.Text = "OUT ARRIVAL";

        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            
        }
    }
}

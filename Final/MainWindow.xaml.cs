using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Final
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void inHex_TextChanged(object sender, TextChangedEventArgs e)
        {
            outErrors.Text = "";
            outHex.Text = "";
            String hexTxt = inHex.Text;

            //Restricting character input to hex characters
            foreach (char c in hexTxt)
            {
                if (!((c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f') || c == ' ' || c == 'x') && hexTxt != String.Empty)
                {
                    inHex.Text = hexTxt.Remove(hexTxt.Length - 1, 1);
                    inHex.SelectionStart = inHex.Text.Length;
                }
            }
        }

        private void calculate_Click(object sender, RoutedEventArgs e)
        {
            outErrors.Text = "";
            outHex.Text = "";

            String hexTxt = inHex.Text.ToLower();
            hexTxt = Regex.Replace(hexTxt, "[^0-9a-f]", "");

            if (hexTxt.Length > 16)
            {
                outErrors.Text = "Please enter a 64 bit hex string for the input!! THIS IS TOO BIG!!!";
                return;
            }

            //Converting hex to ulong
            ulong convert = Convert.ToUInt64(hexTxt, 16);

            //Calculating the given equation based on the publication
            ulong equation = (ROTR(convert, 19)) ^ ROTR(convert, 61) ^ SHR(convert, 6);

            //Padding the hex result to 16 characters
            String result = equation.ToString("X2").PadLeft(16, '0');

            outHex.Text = result;
        }

        ulong ROTR(ulong bits, int cycles)
        {
            /*************************************************************************************************************************
            The ROTR, or Circular Right Shift operation as seen in the FIPS publication. Takes the last bit and moves it to the front.
            *************************************************************************************************************************/
            ulong calc = (bits >> cycles) | (bits << (64 - cycles));
            return calc;
        }

        ulong SHR(ulong bits, int cycles)
        {
            /********************************************************************************************************************************************************
            The SHR, or Right Shift operation as seen in the FIPS publication. Removes the last bit and moves the rest to the right, adding a zero for the first bit.
            ********************************************************************************************************************************************************/
            ulong calc = bits = bits >> cycles;
            return calc;
        }
    }
}

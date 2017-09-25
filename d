[1mdiff --git a/WPFBankApplication/ForgetAccountNumber.xaml b/WPFBankApplication/ForgetAccountNumber.xaml[m
[1mindex 6deefa8..a92b5d1 100644[m
[1m--- a/WPFBankApplication/ForgetAccountNumber.xaml[m
[1m+++ b/WPFBankApplication/ForgetAccountNumber.xaml[m
[36m@@ -7,13 +7,13 @@[m
         xmlns:materialDesign="http://materialdesigninxaml.net/winfx/xaml/themes"[m
         mc:Ignorable="d"[m
         Title="ForgetAccountNumber" Height="368.571" Width="512.214" WindowStartupLocation="CenterScreen" >[m
[31m-    <Grid Background="White">[m
[32m+[m[32m    <Grid>[m
 [m
         <materialDesign:ColorZone Padding="16" materialDesign:ShadowAssist.ShadowDepth="Depth2"[m
                                   Mode="Accent" Height="61">[m
             <Grid Margin="0,0,0,-14" >[m
[31m-                <TextBlock Text="Forget account number" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" Margin="103,0,66,0" ></TextBlock>[m
[31m-                <Button  materialDesign:ShadowAssist.ShadowDepth="Depth0"  BorderBrush="#FF00B8D4" Background="#FF00B8D4"  ToolTip="Go back" Margin="0,-2,391,7" Click="ButtonBase_OnClick" Height="Auto">[m
[32m+[m[32m                <TextBlock Text="Forget Password" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" Margin="133,-7,36,7" ></TextBlock>[m
[32m+[m[32m                <Button Style="{x:Null}" BorderBrush="#FF00B8D4" Background="#FF00B8D4"  ToolTip="Go back" Margin="0,-8,412,10" Click="ButtonBase_OnClick">[m
                     <materialDesign:PackIcon Kind="ArrowLeft" Height="30" Width="44"></materialDesign:PackIcon>[m
                 </Button>[m
             </Grid>[m
[36m@@ -21,14 +21,14 @@[m
 [m
 [m
 [m
[31m-[m
[31m-        <Button Click="ButtonSubmit_OnClick" HorizontalAlignment="Left" Margin="195,281,0,0" VerticalAlignment="Top" Width="154" >[m
[31m-            <Grid>[m
[31m-                <TextBlock Text="SUBMIT" Margin="10,0,-10,0"></TextBlock>[m
[31m-                <materialDesign:PackIcon Kind="Check" Margin="-20,2,0,0" />[m
[31m-            </Grid>[m
[31m-        </Button>[m
[31m-[m
[32m+[m[41m        [m
[32m+[m[32m            <Button Click="ButtonSubmit_OnClick" HorizontalAlignment="Left" Margin="213,279,0,0" VerticalAlignment="Top" Width="111" >[m
[32m+[m[32m                <Grid>[m
[32m+[m[32m                    <TextBlock Text="SUBMIT" Margin="10,0,-10,0"></TextBlock>[m
[32m+[m[32m                    <materialDesign:PackIcon Kind="Check" Margin="-20,2,0,0" />[m
[32m+[m[32m                </Grid>[m
[32m+[m[32m            </Button>[m
[32m+[m[41m        [m
         <TextBlock FontSize="19"  Style="{DynamicResource MaterialDesignSubheadingTextBlock}" HorizontalAlignment="Left" Margin="28,94,0,0" TextWrapping="Wrap" Text="Enter following information to retrive account number" VerticalAlignment="Top" Height="33" Width="458"/>[m
         <TextBox materialDesign:HintAssist.IsFloating="True" Name="TextBoxEmailAddresss" HorizontalAlignment="Left" Height="49" Margin="28,132,0,0" TextWrapping="Wrap" Text="" materialDesign:HintAssist.Hint="Registered email address" VerticalAlignment="Top" Width="452"/>[m
         <PasswordBox materialDesign:HintAssist.IsFloating="True" materialDesign:HintAssist.Hint="Password" Style="{DynamicResource MaterialDesignFloatingHintPasswordBox}" Height="51" HorizontalAlignment="Left"  Name="TextBoxPassword" VerticalAlignment="Top" Width="452" Margin="28,193,0,0" />[m
[1mdiff --git a/WPFBankApplication/LoggedIn.xaml b/WPFBankApplication/LoggedIn.xaml[m
[1mindex ee782f9..e942c18 100644[m
[1m--- a/WPFBankApplication/LoggedIn.xaml[m
[1m+++ b/WPFBankApplication/LoggedIn.xaml[m
[36m@@ -8,7 +8,7 @@[m
     TextElement.FontSize="13"[m
     TextOptions.TextFormattingMode="Ideal" [m
     TextOptions.TextRenderingMode="Auto">[m
[31m-    <Grid Background="#FF78BF81">[m
[32m+[m[32m    <Grid Background="White">[m
 [m
 [m
         <TextBlock[m
[1mdiff --git a/WPFBankApplication/SaveMoney.xaml b/WPFBankApplication/SaveMoney.xaml[m
[1mindex bbf1bb2..1934fcb 100644[m
[1m--- a/WPFBankApplication/SaveMoney.xaml[m
[1m+++ b/WPFBankApplication/SaveMoney.xaml[m
[36m@@ -9,10 +9,10 @@[m
         Title="SaveMoney" Height="333" Width="384" ResizeMode="NoResize" WindowStartupLocation="CenterScreen">[m
     <Grid>[m
         <materialDesign:ColorZone Padding="16" materialDesign:ShadowAssist.ShadowDepth="Depth2"[m
[31m-                                  Mode="Accent" Height="46" Background="#FF209B20" BorderBrush="#FF209B20">[m
[32m+[m[32m                                  Mode="Accent" Height="46">[m
             <Grid Margin="-10,-11,-11,-10" >[m
[31m-                <TextBlock Text="Deposite Money" Style="{DynamicResource MaterialDesignTitleTextBlock}" Margin="67,3,10,0"></TextBlock>[m
[31m-                <Button Margin="0,3,305,0"  materialDesign:ShadowAssist.ShadowDepth="Depth0"  Background="#FF209B20" BorderBrush="#FF209B20" Name="BackButton" Click="BackButton_OnClick"  ToolTip="Go back">[m
[32m+[m[32m                <TextBlock Text="Deposite Money" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" Margin="67,1,10,2"></TextBlock>[m
[32m+[m[32m                <Button Margin="0,3,324,0" Style="{x:Null}" BorderBrush="#FF00B8D4" Background="#FF00B8D4" Name="BackButton" Click="BackButton_OnClick"  ToolTip="Go back">[m
                     <materialDesign:PackIcon Kind="ArrowLeft" Height="22" Width="26"></materialDesign:PackIcon>[m
                 </Button>[m
             </Grid>[m
[1mdiff --git a/WPFBankApplication/TransferMoney.xaml b/WPFBankApplication/TransferMoney.xaml[m
[1mindex 37d0beb..92c6e61 100644[m
[1m--- a/WPFBankApplication/TransferMoney.xaml[m
[1m+++ b/WPFBankApplication/TransferMoney.xaml[m
[36m@@ -11,7 +11,7 @@[m
         <materialDesign:ColorZone Padding="16" materialDesign:ShadowAssist.ShadowDepth="Depth2"[m
                                   Mode="Accent" Height="46">[m
             <Grid Margin="-10,-11,-11,-10" >[m
[31m-                <TextBlock Text="Transfer Money" Style="{DynamicResource MaterialDesignTitleTextBlock}" Margin="105,3,88,0"></TextBlock>[m
[32m+[m[32m                <TextBlock Text="Transfer Money" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" Margin="125,1,68,2"></TextBlock>[m
                 <Button Style="{x:Null}" BorderBrush="#FF00B8D4" Background="#FF00B8D4" Name="BackButton" ToolTip="Go back" Margin="0,0,360,0" Click="BackButton_Click">[m
                     <materialDesign:PackIcon Kind="ArrowLeft" Height="31" Width="40"></materialDesign:PackIcon>[m
                 </Button>[m
[36m@@ -21,7 +21,7 @@[m
         <Button HorizontalAlignment="Left" Margin="130,316,0,0" VerticalAlignment="Top" Width="170" Click="Button_Click">[m
             <Grid>[m
                 <TextBlock Text="Transfer Money"/>[m
[31m-                <materialDesign:PackIcon Kind="Transfer" Margin="-33,2,0,0" Width="26" />[m
[32m+[m[32m                <materialDesign:PackIcon Kind="Transfer" Margin="-35,3,0,0" Width="26" />[m
             </Grid>[m
         </Button>[m
         <TextBox PreviewTextInput="TextBoxAccoutPassword_OnPreviewTextInput" InputScope="Number" Name="TextBoxAccountNumber" Style="{DynamicResource MaterialDesignFloatingHintTextBox}" materialDesign:HintAssist.Hint="Reciver Account number" Height="47" Margin="22,109,24,0" TextWrapping="Wrap" VerticalAlignment="Top"/>[m
[1mdiff --git a/WPFBankApplication/Welcome.xaml b/WPFBankApplication/Welcome.xaml[m
[1mindex 6e3ba13..dbf27a2 100644[m
[1m--- a/WPFBankApplication/Welcome.xaml[m
[1m+++ b/WPFBankApplication/Welcome.xaml[m
[36m@@ -26,12 +26,12 @@[m
 [m
 [m
         </materialDesign:ColorZone >[m
[31m-        <materialDesign:Card Margin="10,45,141,0" Height="353" Background="#FFCAD8FD">[m
[32m+[m[32m        <materialDesign:Card Margin="10,45,141,0" Height="353">[m
             <Grid>[m
[31m-                <materialDesign:Card Height="166" Margin="10,45,216,0">[m
[31m-                    <Image Name="ImageBox" />[m
[32m+[m[32m              <!--  <materialDesign:Card Height="166" Margin="10,45,216,0">[m
[32m+[m[32m                    <Image x:Name="ImageBox" />[m
                 </materialDesign:Card>[m
[31m-[m
[32m+[m[32m-->[m
                 <TextBlock Margin="30,30,10,245" x:Name="TextBlockWelcome" Text="" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" />[m
 [m
             </Grid>[m
[36m@@ -39,39 +39,40 @@[m
 [m
         </materialDesign:Card>[m
 [m
[31m-        <TextBlock HorizontalAlignment="Left" Height="44" Margin="204,184,0,0" TextWrapping="Wrap" Text="Account Summary" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" VerticalAlignment="Top" Width="321"/>[m
[32m+[m
         <materialDesign:Snackbar MessageQueue="{materialDesign:MessageQueue}" x:Name="MainSnackbar"[m
         />[m
         <TextBlock x:Name="TextBlockAccountNo" HorizontalAlignment="Left" Margin="600,29,0,0" TextWrapping="Wrap" VerticalAlignment="Top" Text=""></TextBlock>[m
[31m-        <Button x:Name="ButtonWithdrawMoney" HorizontalAlignment="Left" Margin="636,102,0,0" VerticalAlignment="Top" Width="192" Height="31" Click="ButtonWithdrawMoney_Click" Background="#FFD34B4B" BorderBrush="#FFD34B4B" >[m
[32m+[m[32m        <Button x:Name="ButtonCheckAccountDetails" HorizontalAlignment="Left" Margin="636,112,0,0" VerticalAlignment="Top" Width="192" Height="35" Click="ButtonCheckAccountDetails_Click">[m
[32m+[m[32m            <Grid>[m
[32m+[m[32m                <TextBlock Text="Check account details" Margin="11,0,-11,0"/>[m
[32m+[m[32m                <materialDesign:PackIcon Kind="AccountCheck" Margin="-23,0,0,0" Width="31" Height="19" />[m
[32m+[m[32m            </Grid>[m
[32m+[m[32m        </Button>[m
[32m+[m[32m        <Button x:Name="ButtonWithdrawMoney" HorizontalAlignment="Left" Margin="636,166,0,0" VerticalAlignment="Top" Width="192" Height="31" Click="ButtonWithdrawMoney_Click" >[m
             <Grid>[m
                 <TextBlock Text="Withdraw money"/>[m
                 <materialDesign:PackIcon Kind="ChevronDoubleDown" Margin="-36,0,0,0" Width="31" Height="19" />[m
             </Grid>[m
         </Button>[m
[31m-        <Button x:Name="ButtonSaveMoney" HorizontalAlignment="Left" Margin="636,184,0,0" VerticalAlignment="Top" Width="192" Height="35" Click="ButtonSaveMoney_Click" Background="#FF209B20" BorderBrush="#FF209B20" >[m
[32m+[m[32m        <Button x:Name="ButtonSaveMoney" HorizontalAlignment="Left" Margin="636,222,0,0" VerticalAlignment="Top" Width="192" Height="35" Click="ButtonSaveMoney_Click" >[m
             <Grid>[m
[31m-                <TextBlock Text="Deposite money"/>[m
[31m-                <materialDesign:PackIcon Kind="ChevronDoubleDown" Margin="-43,0,0,0" Width="31" Height="19" />[m
[32m+[m[32m                <TextBlock Text="Save money"/>[m
[32m+[m[32m                <materialDesign:PackIcon Kind="ChevronDoubleDown" Margin="-51,0,0,0" Width="31" Height="19" />[m
             </Grid>[m
         </Button>[m
[31m-        <Button HorizontalAlignment="Left" Margin="636,350,0,0" VerticalAlignment="Top" Width="192" Click="Button_Click" Background="DarkMagenta" BorderBrush="DarkMagenta" >[m
[32m+[m[32m        <Button HorizontalAlignment="Left" Margin="636,346,0,0" VerticalAlignment="Top" Width="192" Click="Button_Click" >[m
             <Grid>[m
                 <TextBlock Text="Account settings"/>[m
                 <materialDesign:PackIcon Kind="AccountSettingsVariant" Margin="-31,3,0,0" Width="26" />[m
             </Grid>[m
         </Button>[m
[31m-        <Button HorizontalAlignment="Left" Margin="636,269,0,0" VerticalAlignment="Top" Width="192" Name="TransferMoneyButton" Click="TransferMoneyButton_OnClick" >[m
[32m+[m[32m        <Button HorizontalAlignment="Left" Margin="636,284,0,0" VerticalAlignment="Top" Width="192" Name="TransferMoneyButton" Click="TransferMoneyButton_OnClick" >[m
             <Grid>[m
                 <TextBlock Text="Transfer Money"/>[m
                 <materialDesign:PackIcon Kind="Transfer" Margin="-35,3,0,0" Width="26" />[m
             </Grid>[m
         </Button>[m
[31m-        <TextBlock HorizontalAlignment="Left" Height="35" Margin="204,222,0,0" TextWrapping="Wrap" Text="Account Number - " Style="{DynamicResource MaterialDesignSubheadingTextBlock}" VerticalAlignment="Top" Width="128"/>[m
[31m-        <TextBlock HorizontalAlignment="Left" Height="35" Margin="204,257,0,0" TextWrapping="Wrap" Text="Available balance - " Style="{DynamicResource MaterialDesignSubheadingTextBlock}" VerticalAlignment="Top" Width="128"/>[m
[31m-        <TextBlock HorizontalAlignment="Left" Name="TextBlockAccountNumber" Height="35" Margin="337,222,0,0" TextWrapping="Wrap" Text="" Style="{DynamicResource MaterialDesignSubheadingTextBlock}" VerticalAlignment="Top" Width="128"/>[m
[31m-        <TextBlock HorizontalAlignment="Left" Name="TextBlockAvaiableBalance" Height="35" Margin="354,257,0,0" TextWrapping="Wrap" Style="{DynamicResource MaterialDesignSubheadingTextBlock}" VerticalAlignment="Top" Width="128"/>[m
[31m-        <TextBlock HorizontalAlignment="Left" Text="Rs" Height="35" Margin="332,257,0,0" TextWrapping="Wrap" Style="{DynamicResource MaterialDesignSubheadingTextBlock}" VerticalAlignment="Top" Width="17"/>[m
 [m
 [m
     </Grid>[m
[1mdiff --git a/WPFBankApplication/Welcome.xaml.cs b/WPFBankApplication/Welcome.xaml.cs[m
[1mindex 486f5a0..05b12ff 100644[m
[1m--- a/WPFBankApplication/Welcome.xaml.cs[m
[1m+++ b/WPFBankApplication/Welcome.xaml.cs[m
[36m@@ -1,7 +1,6 @@[m
 ﻿using System.Windows;[m
 using System.Windows.Media;[m
 using System.Threading.Tasks;[m
[31m-using ExtraTools;[m
 using java.lang;[m
 using java.sql;[m
 using Connection = com.mysql.jdbc.Connection;[m
[36m@@ -11,16 +10,15 @@[m [mnamespace WPFBankApplication[m
     /// <summary>[m
     /// Interaction logic for Welcome.xaml[m
     /// </summary>[m
[31m-    public partial class Welcome[m
[32m+[m[32m    public partial class Welcome : Window[m
     {[m
[31m-        public static string accountNumber = string.Empty;[m
[32m+[m[32m        private static string accountNumber = string.Empty;[m
         public Welcome(string accountNum)[m
         {[m
             InitializeComponent();[m
             accountNumber = accountNum;[m
             TextBlockWelcome.Text = "Hello " + Operations.GetAccountHolderName(accountNumber);[m
[31m-            TextBlockAccountNumber.Text = accountNum;[m
[31m-            TextBlockAvaiableBalance.Text = Operations.GetCurrentbalance(accountNum);[m
[32m+[m[32m            TextBlockAccountNo.Text = accountNum;[m
             GetAccountHolderImage();[m
             ShowWelcomeSnakbar();[m
         }[m
[36m@@ -55,7 +53,7 @@[m [mnamespace WPFBankApplication[m
                 }[m
 [m
                 ImageSourceConverter img = new ImageSourceConverter();[m
[31m-               ImageBox.SetValue(System.Windows.Controls.Image.SourceProperty, img.ConvertFromString(imageFilePath));[m
[32m+[m[32m               // ImageBox.SetValue(System.Windows.Controls.Image.SourceProperty, img.ConvertFromString(imageFilePath));[m
             }[m
             catch (SQLException exception)[m
             {[m
[36m@@ -95,16 +93,8 @@[m [mnamespace WPFBankApplication[m
 [m
         private void ButtonLogOut_OnClick(object sender, RoutedEventArgs e)[m
         {[m
[31m-            int result = (int)DialogBox.Show("Log out ?", "Are you sure you want to log out?", "YES", "NO");[m
[31m-            [m
[31m-            switch (DialogBox.Result)[m
[31m-            {[m
[31m-                case DialogBox.ResultEnum.LeftButtonClicked:[m
[31m-                    new LoggedIn().Show();[m
[31m-                    this.Hide();[m
[31m-                    break;[m
[31m-            }[m
[31m-[m
[32m+[m[32m           new LoggedIn().Show();[m
[32m+[m[32m            this.Hide();[m
         }[m
     }[m
 [m
[1mdiff --git a/WPFBankApplication/WithdrawMoney.xaml b/WPFBankApplication/WithdrawMoney.xaml[m
[1mindex bfe1462..7d4ca15 100644[m
[1m--- a/WPFBankApplication/WithdrawMoney.xaml[m
[1m+++ b/WPFBankApplication/WithdrawMoney.xaml[m
[36m@@ -10,10 +10,10 @@[m
 [m
     <Grid>[m
         <materialDesign:ColorZone Padding="16" materialDesign:ShadowAssist.ShadowDepth="Depth2"[m
[31m-                                  Mode="Accent" Height="46" Background="#FFD34B4B" BorderBrush="#FFD34B4B">[m
[32m+[m[32m                                  Mode="Accent" Height="46">[m
             <Grid Margin="-10,-11,-11,-10" >[m
[31m-                <TextBlock Text="Withdraw money" Style="{DynamicResource MaterialDesignTitleTextBlock}" Margin="67,5,10,-2"></TextBlock>[m
[31m-                <Button Margin="0,2,304,1" materialDesign:ShadowAssist.ShadowDepth="Depth0" BorderBrush="#FFD34B4B" Background="#FFD34B4B" Name="BackButton" Click="BackButton_OnClick" >[m
[32m+[m[32m                <TextBlock Text="Withdraw Money" Style="{DynamicResource MaterialDesignHeadlineTextBlock}" Margin="67,1,10,2"></TextBlock>[m
[32m+[m[32m                <Button Margin="0,3,320,0" Style="{x:Null}" BorderBrush="#FF00B8D4" Background="#FF00B8D4" Name="BackButton" Click="BackButton_OnClick" >[m
                     <materialDesign:PackIcon Kind="ArrowLeft" Height="28" Width="36"></materialDesign:PackIcon>[m
                 </Button>[m
             </Grid>[m

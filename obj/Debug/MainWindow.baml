   M S B A M L   `   `   `  ����H  DHeeckerenTest, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null-clr-namespace:HeeckerenTestHeeckerenTest  5   	       HeeckerenTest.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   -localclr-namespace:HeeckerenTest     5   	     � Title$   HeeckerenTest v 3.20.10.12��5	   	   $	��600��6,   $	��900��69   $	��750��6E   $	��500��6T    � Icon$ 	Hicon.ico��6d   .��5      � $��10,10,10,10q�6   ���	#FFF6F6F6 J� Effect 5   
   3  -System.Windows.Media.Effects.DropShadowEffect  5         	Direction$ 0Q�6      
BlurRadius$ 20Q�6-      ShadowDepth$ 0Q�6=      Opacity$	 0.5Q�6M    � ColumnDefinitions 5   
   �� 5      $��80 �6   �� 5      $	��300��6   �� 5      $	��300 �6    � RowDefinitions 5   
   �� 5      $��88 �6   �� 5      �� 5      $��20 �6   .+�5   
   �� -   5        	 ��Name$	 	Statistic��5      $��	4,12,4,12q�6%   $��1��68   $��0��6H   $��2��6U   ��. 
�������6�   �� 5   
   -   5        $	 Result��5      $��
4,12,12,12q�6"   $��2��66   $��0��6F   $��2��6S   ��. 
�������
 �� SelectionMode$
 Single=�6�   6�   �� 5   
   -   5         ��Name$ 	StartStop��5      $
��8,12q�6$   $��0��62   $
��Left=�6B   $	��Top=�6]   $��0��6u   $��0��6   $��64��6�   $��64��6�   $��START��6�   $��Disabled=�6�    5        5      $	 0.2Q�6#   $ 0Q�61   $ 10Q�6?   $ 0Q�6O   5      �� 5    
   $��1��6   $��0��6    J� RenderTransformOrigin$ 0.5,0&�6-   $��Filter��6K   $��0q�6\   $��Center=�6g   �� 5!   
   -   5        $��FilterLabel��5!      $��1��6#   $��0��60   $ 0.5,0&�6@   $	��500��6^   $��8,16,8,8q�6l   $��Center=�6~   �� 5"   
   -   5         � Value$ 5Q�5"       � Minimum$ 0Q�6    � Maximum$ 20Q�6'    �� SelectionStart$ 0Q�64   $��1��6G   $��0��6T   $ 0.5,0&�6d   $��Center=�6�   $	��Top=�6�   $	��250��6�    �� IsSelectionRangeEnabled .  �� Orientation$ Vertical=�5#   S   $��0,50,0,0q�6j   � 5$   
   -   5         �Name$ Progress��5$      $ 0Q�6(   $	 100Q�64   $ 50Q�6B   $��0��6M   $��2��6]   $��3��6j   $��0,15,0,0q�6~   ��6�   Z� 5%      5&      
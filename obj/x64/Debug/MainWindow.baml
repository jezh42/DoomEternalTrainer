   M S B A M L   `   `   `  ����M  IDoomEternalTrainer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:DoomTrainerDoomTrainer  5   	       DoomTrainer.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   +localclr-namespace:DoomTrainer     5   	     � Title$   Doom Eternal Trainer 1.1.5��5   	   $��231.92��6,   $��464.884��6<    � 
ResizeMode$ NoResize=�6L   .��5	      � $��0,0,2,0q�6    � ColumnDefinitions 5
      �� 5      .+�5      �� -   5         ��Name$ PosBlock��5      $
��Left=�6    $	��206��6;   $��	10,12,0,0q�6H   $
��Wrap=�6[   $	��Top=�6o   $	��119��6�   �� 5      -   5         ��Name$ unlockButton��5      $��Unlock Console Commands��6!   $
��Left=�6C   $��53��6^   $��
263,12,0,0q�6j   $	��Top=�6~   $	��171��6�   6�   �� 5      -   5        $ VelBlock��5      $
��Left=�6    $	��206��6;   $��
134,12,0,0q�6H   $
��Wrap=�6\   $	��Top=�6p   $	��119��6�   �� 5      $��Velocity Behavior��6
   $
��Left=�6&   $��28��6A   $��258,104,0,0q�6M   $	��Top=�6b   $	��169��6z   �� 5      -   5         ��Name$ zeroRB��5      $��CheckBox��6   $
��Left=�60   $��16��6K   $��263,132,0,0q�6W   $	��Top=�6l   $��16��6�    J� RenderTransformOrigin$ -0.203,-0.874&�6�    d� 	IsChecked$
 TrueY�6�   6�   �� 5      -   5        $ retainRB��5      $��CheckBox��6   $
��Left=�62   $��16��6M   $��263,153,0,0q�6Y   $	��Top=�6n   $��16��6�   $ -0.203,-0.874&�6�   6�   �� 5      $��Set to 0��6
   $
��Left=�6   $��21��68   $��284,128,0,0q�6D   $	��Top=�6Y   $	��145��6q   $��10(�6}   �� 5      $��
Use Stored��6
   $
��Left=�6   $��21��6:   $��284,149,0,0q�6F   $	��Top=�6[   $	��145��6s   $��10(�6   �� 5      -   5        $ 	showposCB��5      $��CheckBox��6    $
��Left=�63   $��16��6N   $��
263,75,0,0q�6Z   $	��Top=�6n   $��16��6�   $
 TrueY�6�   �� 5      $��Show readouts in-game��6
   $
��Left=�6*   $��26��6E   $��
284,71,0,0q�6Q   $	��Top=�6e   $	��145��6}   $��10(�6�   5      5      
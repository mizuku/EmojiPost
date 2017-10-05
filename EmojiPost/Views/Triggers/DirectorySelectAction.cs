using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace AyaStyle.Views
{
    /// <summary>
    /// ディレクトリ選択トリガーアクション
    /// </summary>
    public class DirectorySelectAction : TriggerAction<UIElement>
    {

        #region DependencyPropertieds

        /// <summary>
        /// ディレクトリ選択完了コマンド を取得または設定します。
        /// </summary>
        public ICommand CompleteCommand 
        {
            get { return (ICommand)GetValue(CompleteCommandProperty); }
            set { SetValue(CompleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompleteCommandProperty =
            DependencyProperty.Register("CompleteCommand", typeof(ICommand), typeof(DirectorySelectAction), new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, (oo, ee) => { }, null, false, UpdateSourceTrigger.PropertyChanged));

        #endregion

        protected override void Invoke(object parameter)
        {
            // WPF標準のファイルダイアログにはフォルダー選択モードが無いので WindowsAPICodePackを使う
            var dialog = new CommonOpenFileDialog()
            {
                EnsurePathExists = true,
                EnsureValidNames = true,
                Multiselect = false,
                IsFolderPicker = true,
                //DefaultDirectory = "",
                Title = "保存フォルダーを選択",
            };
            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var command = this.CompleteCommand;
                if (null != command && command.CanExecute(dialog.FileName))
                {
                    command.Execute(dialog.FileName);
                }
            }
        }

    }
}

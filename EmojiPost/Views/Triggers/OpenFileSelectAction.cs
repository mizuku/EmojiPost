using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Interactivity;
using Microsoft.Win32;

namespace AyaStyle.Views
{
    /// <summary>
    /// ファイル選択トリガーアクション
    /// </summary>
    public class OpenFileSelectAction : TriggerAction<UIElement>
    {

        #region DependencyPropertieds

        /// <summary>
        /// ファイル選択完了コマンド を取得または設定します。
        /// </summary>
        public ICommand CompleteCommand 
        {
            get { return (ICommand)GetValue(CompleteCommandProperty); }
            set { SetValue(CompleteCommandProperty, value); }
        }

        // Using a DependencyProperty as the backing store for CompleteCommand.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty CompleteCommandProperty =
            DependencyProperty.Register("CompleteCommand", typeof(ICommand), typeof(OpenFileSelectAction), new FrameworkPropertyMetadata(
                null, FrameworkPropertyMetadataOptions.SubPropertiesDoNotAffectRender, (oo, ee) => { }, null, false, UpdateSourceTrigger.PropertyChanged));

        #endregion

        protected override void Invoke(object parameter)
        {
            var dialog = new OpenFileDialog()
            {
                CheckFileExists = true,
                CheckPathExists = true,
                DereferenceLinks = true,
                //InitialDirectory = "",
                Title = "スタンプ画像を選択",
                ValidateNames = true,
            };
            if (dialog.ShowDialog() ?? false)
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

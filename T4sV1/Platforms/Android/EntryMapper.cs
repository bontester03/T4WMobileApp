using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Handlers;

namespace T4sV1.Platforms.Android
{
    public static class EntryMapper
    {
        public static void RemoveUnderline()
        {
            // Remove underline from Entry
            EntryHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                if (handler.PlatformView is AndroidX.AppCompat.Widget.AppCompatEditText editText)
                {
                    editText.BackgroundTintList = global::Android.Content.Res.ColorStateList.ValueOf(global::Android.Graphics.Color.Transparent);
                }
            });

            // Remove underline from Picker (Dropdown)
            PickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                if (handler.PlatformView is global::Android.Widget.EditText editText)
                {
                    editText.BackgroundTintList = global::Android.Content.Res.ColorStateList.ValueOf(global::Android.Graphics.Color.Transparent);
                }
            });

            // Remove underline from DatePicker
            DatePickerHandler.Mapper.AppendToMapping("NoUnderline", (handler, view) =>
            {
                if (handler.PlatformView is global::Android.Widget.EditText editText)
                {
                    editText.BackgroundTintList = global::Android.Content.Res.ColorStateList.ValueOf(global::Android.Graphics.Color.Transparent);
                }
            });
        }
    }
}
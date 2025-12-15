using SkiaSharp;
using SkiaSharp.Views.Maui;
using SkiaSharp.Views.Maui.Controls;
using System.Diagnostics;

namespace T4sV1.Controls;

public class SvgImage : SKCanvasView
{
    public SvgImage()
    {
        Debug.WriteLine("🎨 SvgImage: Constructor called");
    }

    public static readonly BindableProperty SourceProperty = BindableProperty.Create(
        nameof(Source),
        typeof(string),
        typeof(SvgImage),
        defaultValue: string.Empty,
        propertyChanged: OnSourceChanged);

    public string Source
    {
        get => (string)GetValue(SourceProperty);
        set
        {
            Debug.WriteLine($"🎨 SvgImage: Source setter called with: {value}");
            SetValue(SourceProperty, value);
        }
    }

    private static void OnSourceChanged(BindableObject bindable, object oldValue, object newValue)
    {
        Debug.WriteLine($"🎨 SvgImage: OnSourceChanged - Old: {oldValue}, New: {newValue}");

        if (bindable is SvgImage svgImage && newValue is string source && !string.IsNullOrEmpty(source))
        {
            Debug.WriteLine($"🎨 SvgImage: Starting LoadSvgAsync for {source}");
            _ = svgImage.LoadSvgAsync(source);
        }
    }

    private SKSvg? _svg;

    protected override void OnPaintSurface(SKPaintSurfaceEventArgs e)
    {
        base.OnPaintSurface(e);

        var canvas = e.Surface.Canvas;
        canvas.Clear(SKColors.Transparent);

        if (_svg?.Picture != null)
        {
            var info = e.Info;

            // Use the SVG's viewBox dimensions
            var svgSize = _svg.Picture.CullRect.Size;

            Debug.WriteLine($"🎨 OnPaintSurface - Canvas: {info.Width}x{info.Height}, SVG Size: {svgSize}");

            if (svgSize.Width > 0 && svgSize.Height > 0)
            {
                // Calculate scale
                var scaleX = info.Width / svgSize.Width;
                var scaleY = info.Height / svgSize.Height;
                var scale = Math.Min(scaleX, scaleY);

                canvas.Save();

                // Center and scale
                var translateX = (info.Width - svgSize.Width * scale) / 2f;
                var translateY = (info.Height - svgSize.Height * scale) / 2f;

                canvas.Translate(translateX, translateY);
                canvas.Scale(scale);

                // Draw using a white background to check if colors are inverted
                using (var paint = new SKPaint { Color = SKColors.White })
                {
                    canvas.DrawRect(0, 0, svgSize.Width, svgSize.Height, paint);
                }

                canvas.DrawPicture(_svg.Picture);

                canvas.Restore();

                Debug.WriteLine($"🎨 Drawn with scale {scale}");
            }
        }
    }

    private async Task LoadSvgAsync(string source)
    {
        try
        {
            if (string.IsNullOrEmpty(source))
            {
                Debug.WriteLine("🎨 SvgImage: Source is null or empty");
                return;
            }

            Debug.WriteLine($"🎨 SvgImage: Loading {source}");

            // Run the network request on a background thread
            await Task.Run(async () =>
            {
                try
                {
                    var svg = new SKSvg();

                    if (source.StartsWith("http://") || source.StartsWith("https://"))
                    {
                        using var httpClient = new HttpClient();
                        httpClient.Timeout = TimeSpan.FromSeconds(10);

                        Debug.WriteLine($"🎨 SvgImage: Downloading {source}");
                        var svgBytes = await httpClient.GetByteArrayAsync(source);
                        Debug.WriteLine($"🎨 SvgImage: Downloaded {svgBytes.Length} bytes");

                        using var stream = new MemoryStream(svgBytes);
                        svg.Load(stream);
                        Debug.WriteLine($"🎨 SvgImage: SVG parsed successfully");
                    }
                    else
                    {
                        svg.Load(source);
                    }

                    // Update on main thread
                    await MainThread.InvokeOnMainThreadAsync(() =>
                    {
                        _svg = svg;
                        InvalidateSurface();
                        Debug.WriteLine($"✅ SvgImage: Successfully loaded and invalidated {source}");
                    });
                }
                catch (Exception innerEx)
                {
                    Debug.WriteLine($"❌ SvgImage: Error in background task: {innerEx.Message}");
                }
            });
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"❌ SvgImage: Error loading {source}: {ex.Message}");
            _svg = null;
        }
    }
}
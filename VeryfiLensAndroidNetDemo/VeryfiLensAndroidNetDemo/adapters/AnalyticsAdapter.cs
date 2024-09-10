using Android.Views;
using AndroidX.RecyclerView.Widget;

namespace VeryfiLensAndroidNetDemo.adapters;

public class AnalyticsAdapter : RecyclerView.Adapter
{
    private List<(string EventValue, string ParamsValue, string Value)> items;

    public AnalyticsAdapter(List<(string EventValue, string ParamsValue, string Value)> items)
    {
        this.items = items;
    }

    public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
    {
        View itemView = LayoutInflater.From(parent.Context)
            .Inflate(Resource.Layout.cardview_analytics_item, parent, false);
        return new AnalyticsViewHolder(itemView);
    }

    public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
    {
        var item = items[position];
        var analyticsHolder = holder as AnalyticsViewHolder;
        analyticsHolder.TextView.Text = $"{item.EventValue}";
        analyticsHolder.TextViewSource.Text =
            string.IsNullOrEmpty(item.Value) ? "No additional parameters" : $"{item.ParamsValue + ": " + item.Value}";
    }

    public override int ItemCount => items.Count;
}

public class AnalyticsViewHolder : RecyclerView.ViewHolder
{
    public TextView TextView { get; private set; } = null!;
    public TextView TextViewSource { get; private set; } = null!;
    
    public AnalyticsViewHolder(View itemView) : base(itemView)
    {
        TextView = itemView.FindViewById<TextView>(Resource.Id.textViewAnalyticsItem)!;
        TextViewSource = itemView.FindViewById<TextView>(Resource.Id.textViewAnalyticsItemSource)!;
    }
}
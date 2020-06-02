using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FeatureDemo.Common {
    public enum TileSize { Large, Wide, Small };
    
    public class VerticalTilePanel : Panel {
        #region Dependency Properties
        public static readonly DependencyProperty SmallTileHeightProperty;
        public static readonly DependencyProperty SmallTileWidthProperty;
        public static readonly DependencyProperty TileSizeProperty;
        public static readonly DependencyProperty ColumnProperty;
        public static readonly DependencyProperty RowProperty;
        public static int GetRow(DependencyObject obj) {
            return (int)obj.GetValue(RowProperty);
        }

        public static void SetRow(DependencyObject obj, int value) {
            obj.SetValue(RowProperty, value);
        }

        

        public static int GetColumn(DependencyObject obj) {
            return (int)obj.GetValue(ColumnProperty);
        }

        public static void SetColumn(DependencyObject obj, int value) {
            obj.SetValue(ColumnProperty, value);
        }
        
        static VerticalTilePanel() {
            Type ownerType = typeof(VerticalTilePanel);
            SmallTileHeightProperty = DependencyProperty.Register("SmallTileHeight", typeof(double), ownerType, new PropertyMetadata(100.0, InvalidateMeasure));
            SmallTileWidthProperty = DependencyProperty.Register("SmallTileWidth", typeof(double), ownerType, new PropertyMetadata(300.0, InvalidateMeasure));
            TileSizeProperty = DependencyProperty.RegisterAttached("TileSize", typeof(TileSize), ownerType, new PropertyMetadata(TileSize.Large));
            ColumnProperty = DependencyProperty.RegisterAttached("Column", typeof(int), ownerType, new PropertyMetadata(0));
            RowProperty = DependencyProperty.RegisterAttached("Row", typeof(int), ownerType, new PropertyMetadata(0));
        }
        public static TileSize GetTileSize(DependencyObject obj) {
            return (TileSize)obj.GetValue(TileSizeProperty);
        }
        public static void SetTileSize(DependencyObject obj, TileSize value) {
            obj.SetValue(TileSizeProperty, value);
        }
        private static void InvalidateMeasure(DependencyObject d, DependencyPropertyChangedEventArgs e) {
            ((VerticalTilePanel)d).InvalidateMeasure();
        }
        #endregion

        public double SmallTileHeight { get { return (double)GetValue(SmallTileHeightProperty); } set { SetValue(SmallTileHeightProperty, value); } }
        public double SmallTileWidth { get { return (double)GetValue(SmallTileWidthProperty); } set { SetValue(SmallTileWidthProperty, value); } }

        protected int LayoutTileColumn(int columnIndex, int startIndex, int maxVerticalCellCount, int maxHorizontalCellCount, ref int rowCount) {
            maxVerticalCellCount = Math.Max(2, maxVerticalCellCount);
            int currentColumn = 0;
            int currentRow = 0;
            int rowSpan = 0;
            rowCount = 0;
            for(int i = startIndex; i < Children.Count; i++) {
                TileSize tileSize = VerticalTilePanel.GetTileSize(Children[i]);                
                int columnSpan = tileSize == TileSize.Small ? 1 : 2;
                if(columnSpan > maxHorizontalCellCount)
                    return -1;                
                if(currentColumn + columnSpan > maxHorizontalCellCount) {
                    currentRow += rowSpan;
                    rowCount += rowSpan;
                    currentColumn = 0;
                }
                rowSpan = tileSize == TileSize.Large ? 2 : 1;
                if(currentRow + rowSpan > maxVerticalCellCount)
                    return i - 1;
                VerticalTilePanel.SetColumn(Children[i], columnIndex + currentColumn);
                VerticalTilePanel.SetRow(Children[i], currentRow);
                currentColumn += columnSpan;
            }
            
            return Children.Count - 1;
        }

        protected override Size MeasureOverride(Size availableSize) {
            int maxVerticalCellCount = Math.Max(1, (int)(double.IsInfinity(availableSize.Height) ? 100 : availableSize.Height / SmallTileHeight));
            int startIndex = 0;
            int columnIndex = 0;
            int rowCount = 0;
            while(startIndex != Children.Count) {
                int newIndex = LayoutTileColumn(columnIndex, startIndex, maxVerticalCellCount, 1, ref rowCount);
                columnIndex++;
                if(newIndex == -1) {
                    newIndex = LayoutTileColumn(columnIndex - 1, startIndex, maxVerticalCellCount, 2, ref rowCount);
                    if(newIndex == -1)
                        throw new InvalidOperationException();
                    columnIndex ++;
                }
                if(startIndex == 0)
                    maxVerticalCellCount = Math.Min(rowCount, maxVerticalCellCount);
                startIndex = newIndex + 1;
            }
            foreach(UIElement child in Children) {
                TileSize tileSize = VerticalTilePanel.GetTileSize(child);
                int rowSpan = tileSize == TileSize.Large ? 2 : 1;
                int columnSpan = tileSize == TileSize.Small ? 1 : 2;                
                child.Measure(new Size(columnSpan * SmallTileWidth, rowSpan * SmallTileHeight));
            }
            return new Size(SmallTileWidth * columnIndex, SmallTileHeight * maxVerticalCellCount);
        }
        protected override Size ArrangeOverride(Size finalSize) {
            foreach(UIElement child in Children) {
                TileSize tileSize = VerticalTilePanel.GetTileSize(child);
                int rowSpan = tileSize == TileSize.Large ? 2 : 1;
                int columnSpan = tileSize == TileSize.Small ? 1: 2;                
                child.Arrange(new Rect(VerticalTilePanel.GetColumn(child) * SmallTileWidth, VerticalTilePanel.GetRow(child) * SmallTileHeight, columnSpan * SmallTileWidth, rowSpan * SmallTileHeight));
            }
            return finalSize;
        }
    }
}

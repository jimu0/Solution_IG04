namespace Mycelia;

public class Grid2D
{
    // public int Width { get; }
    // public int Height { get; }
    // public float CellSize { get; }
    // public Color[,] Cells { get; }
    //
    // public Grid2D(int width, int height, float cellSize)
    // {
    //     Width = width;
    //     Height = height;
    //     CellSize = cellSize;
    //     Cells = new Color[width, height];
    //     
    //     for (int x = 0; x < width; x++)
    //     {
    //         for (int y = 0; y < height; y++)
    //         {
    //             Cells[x, y] = Color.Transparent;
    //         }
    //     }
    // }
    //
    // // 获取单元格的中心世界坐标
    // public PointF GetCellCenterWorld(int x, int y)
    // {
    //     return new PointF(x * CellSize + CellSize / 2, y * CellSize + CellSize / 2);
    // }
    //
    // // 获取单元格的世界坐标矩形
    // public RectangleF GetCellRectWorld(int x, int y)
    // {
    //     return new RectangleF(x * CellSize, y * CellSize, CellSize, CellSize);
    // }
    //
    // // 获取单元格的屏幕坐标矩形
    // public RectangleF? GetCellRectScreen(int x, int y, Camera2D camera, Size viewportSize)
    // {
    //     RectangleF worldRect = GetCellRectWorld(x, y);
    //     PointF screenTopLeft = camera.WorldToScreen(new PointF(worldRect.Left, worldRect.Top), viewportSize);
    //     PointF screenBottomRight = camera.WorldToScreen(new PointF(worldRect.Right, worldRect.Bottom), viewportSize);
    //     return new RectangleF(screenTopLeft.X, screenTopLeft.Y, screenBottomRight.X - screenTopLeft.X, screenBottomRight.Y - screenTopLeft.Y);
    // }
    //
    // // 从世界坐标获取网格坐标
    // public bool TryGetGridPosition(PointF worldPoint, out int gridX, out int gridY)
    // {
    //     gridX = (int)(worldPoint.X / CellSize);
    //     gridY = (int)(worldPoint.Y / CellSize);
    //     return gridX >= 0 && gridX < Width && gridY >= 0 && gridY < Height;
    // }
    //
    // // 从屏幕坐标获取网格坐标
    // public bool TryGetGridPositionFromScreen(PointF screenPoint, Camera2D camera, Size viewportSize, out int gridX, out int gridY)
    // {
    //     PointF worldPoint = camera.ScreenToWorld(screenPoint, viewportSize);
    //     return TryGetGridPosition(worldPoint, out gridX, out gridY);
    // }
}


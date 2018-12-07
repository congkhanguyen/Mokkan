namespace MokkAnnotator.MkaCommon
{
    /// <summary>
    /// Work type
    /// </summary>
    public enum WorkType
    {     
        None,
        New,     
        Load
    }

    /// <summary>
    /// Image zoom type
    /// </summary>
    public enum ImageZoomType
    {
        ZoomIn,         
        ZoomOut,        
        ZoomToWindow,   
        ZoomActual     
    }

    /// <summary>
    /// Display type
    /// </summary>
    public enum DisplayType
    {
        All,        // view all
        ImageOnly,  // view image only
        DataOnly    // view items only
    }

    /// <summary>
    /// Tool type
    /// </summary>
    public enum ToolType
    {
        Pointer,    
        Rectangle,  
        Ellipse,    
        Polygon,    
        ToolsCount
    }

    /// <summary>
    /// Show remain id position
    /// </summary>
    public enum ShowPosition
    {
        Top,       
        Bottom
    }

    /// <summary>
    /// Selected object types
    /// </summary>
    public enum SelectedObjectType
    {
        Bat,
        Glass,
        Mokkan,
        None
    }

    /// <summary>
    /// Rotate image types
    /// </summary>
    public enum RotateType
    {
        NearestNeighbor,
        Bilinear,
        Bicubic
    }

    /// <summary>
    /// Auto select method
    /// </summary>
    public enum AutoSelectType
    {
        HSL,
        YCbrCr,
        Thresholding
    }
}
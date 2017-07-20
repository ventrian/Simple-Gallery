'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports Ventrian.SimpleGallery.Common

Namespace Ventrian.SimpleGallery.Entities

    Public Class GallerySettings

#Region " Private Members "

        Private _settings As Hashtable

#End Region

#Region " Constructors "

        Sub New(ByVal settings As Hashtable)

            _settings = settings

        End Sub

#End Region

#Region " Public Properties "

        Public ReadOnly Property JavascriptVersion() As String
            Get
                Return Constants.JAVASCRIPT_VERSION
            End Get
        End Property

        Public ReadOnly Property AlbumDefaultPath() As String
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_DEFAULT_PATH)) Then
                    Return _settings(Constants.SETTING_ALBUM_DEFAULT_PATH).ToString()
                Else
                    Return Constants.DEFAULT_ALBUM_DEFAULT_PATH
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxDefaultPath() As String
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_DEFAULT_PATH)) Then
                    Return _settings(Constants.SETTING_LIGHTBOX_DEFAULT_PATH).ToString()
                Else
                    Return Constants.DEFAULT_LIGHTBOX_DEFAULT_PATH
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumThumbnailHeight() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_THUMBNAIL_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_ALBUM_THUMBNAIL_HEIGHT).ToString())
                Else
                    Return Constants.DEFAULT_ALBUM_THUMBNAIL_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumThumbnailWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_THUMBNAIL_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_ALBUM_THUMBNAIL_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_ALBUM_THUMBNAIL_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumThumbnailSquare() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_THUMBNAIL_SQUARE)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_ALBUM_THUMBNAIL_SQUARE).ToString())
                Else
                    Return Constants.DEFAULT_ALBUM_THUMBNAIL_SQUARE
                End If
            End Get
        End Property

        Public ReadOnly Property Compression() As CompressionType
            Get
                If (_settings.Contains(Constants.SETTING_COMPRESSION)) Then
                    Return CType(System.Enum.Parse(GetType(CompressionType), _settings(Constants.SETTING_COMPRESSION).ToString()), CompressionType)
                Else
                    Return Constants.DEFAULT_COMPRESSION
                End If
            End Get
        End Property

        Public ReadOnly Property CompressionAlbum() As CompressionType
            Get
                If (_settings.Contains(Constants.SETTING_COMPRESSION_ALBUM)) Then
                    Return CType(System.Enum.Parse(GetType(CompressionType), _settings(Constants.SETTING_COMPRESSION_ALBUM).ToString()), CompressionType)
                Else
                    Return Constants.DEFAULT_COMPRESSION_ALBUM
                End If
            End Get
        End Property

        Public ReadOnly Property CompressionPhoto() As CompressionType
            Get
                If (_settings.Contains(Constants.SETTING_COMPRESSION_PHOTO)) Then
                    Return CType(System.Enum.Parse(GetType(CompressionType), _settings(Constants.SETTING_COMPRESSION_PHOTO).ToString()), CompressionType)
                Else
                    Return Constants.DEFAULT_COMPRESSION_PHOTO
                End If
            End Get
        End Property

        Public ReadOnly Property ThumbnailAlbum() As ThumbnailType
            Get
                If (_settings.Contains(Constants.SETTING_THUMBNAIL_ALBUM)) Then
                    Return CType(System.Enum.Parse(GetType(ThumbnailType), _settings(Constants.SETTING_THUMBNAIL_ALBUM).ToString()), ThumbnailType)
                Else
                    Return Constants.DEFAULT_THUMBNAIL_ALBUM
                End If
            End Get
        End Property

        Public ReadOnly Property ThumbnailPhoto() As ThumbnailType
            Get
                If (_settings.Contains(Constants.SETTING_THUMBNAIL_PHOTO)) Then
                    Return CType(System.Enum.Parse(GetType(ThumbnailType), _settings(Constants.SETTING_THUMBNAIL_PHOTO).ToString()), ThumbnailType)
                Else
                    Return Constants.DEFAULT_THUMBNAIL_PHOTO
                End If
            End Get
        End Property

        Public ReadOnly Property ImageHeight() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_HEIGHT).ToString())
                Else
                    Return Constants.DEFAULT_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property ImageWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property ResizePhoto() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_RESIZE_PHOTO)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_RESIZE_PHOTO).ToString())
                Else
                    Return Constants.DEFAULT_RESIZE_PHOTO
                End If
            End Get
        End Property

        Public ReadOnly Property Slideshow() As SlideshowType
            Get
                If (_settings.Contains(Constants.SETTING_SLIDESHOW_TYPE)) Then
                    Return CType(System.Enum.Parse(GetType(SlideshowType), _settings(Constants.SETTING_SLIDESHOW_TYPE).ToString()), SlideshowType)
                Else
                    Return Constants.DEFAULT_SLIDESHOW_TYPE
                End If
            End Get
        End Property

        Public ReadOnly Property ThumbnailWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_THUMBNAIL_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_THUMBNAIL_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_THUMBNAIL_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property ThumbnailHeight() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_THUMBNAIL_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_THUMBNAIL_HEIGHT).ToString())
                Else
                    Return Constants.DEFAULT_THUMBNAIL_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property ThumbnailSquare() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_THUMBNAIL_SQUARE)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_THUMBNAIL_SQUARE).ToString())
                Else
                    Return Constants.DEFAULT_THUMBNAIL_SQUARE
                End If
            End Get
        End Property

        Public ReadOnly Property UseAlbumAnchors() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_USE_ALBUM_ANCHORS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_USE_ALBUM_ANCHORS).ToString())
                Else
                    Return Constants.DEFAULT_USE_ALBUM_ANCHORS
                End If
            End Get
        End Property

        Public ReadOnly Property UseWatermark() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_USE_WATERMARK)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_USE_WATERMARK).ToString())
                Else
                    Return Constants.DEFAULT_USE_WATERMARK
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkText() As String
            Get
                If (_settings.Contains(Constants.SETTING_WATERMARK_TEXT)) Then
                    Return _settings(Constants.SETTING_WATERMARK_TEXT).ToString()
                Else
                    Return Constants.DEFAULT_WATERMARK_TEXT
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkImage() As String
            Get
                If (_settings.Contains(Constants.SETTING_WATERMARK_IMAGE)) Then
                    Return _settings(Constants.SETTING_WATERMARK_IMAGE).ToString()
                Else
                    Return Constants.DEFAULT_WATERMARK_IMAGE
                End If
            End Get
        End Property

        Public ReadOnly Property WatermarkImagePosition() As WatermarkPosition
            Get
                If (_settings.Contains(Constants.SETTING_WATERMARK_IMAGE_POSITION)) Then
                    Return CType(System.Enum.Parse(GetType(WatermarkPosition), _settings(Constants.SETTING_WATERMARK_IMAGE_POSITION).ToString()), WatermarkPosition)
                Else
                    Return Constants.DEFAULT_WATERMARK_IMAGE_POSITION
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSearch() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ENABLE_SEARCH)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ENABLE_SEARCH).ToString())
                Else
                    Return Constants.DEFAULT_ENABLE_SEARCH
                End If
            End Get
        End Property

        Public ReadOnly Property EnableSyndication() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ENABLE_SYNDICATION)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ENABLE_SYNDICATION).ToString())
                Else
                    Return Constants.DEFAULT_ENABLE_SYNDICATION
                End If
            End Get
        End Property

        Public ReadOnly Property EnableTooltip() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ENABLE_TOOLTIP)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ENABLE_TOOLTIP).ToString())
                Else
                    Return Constants.DEFAULT_ENABLE_TOOLTIP
                End If
            End Get
        End Property

        Public ReadOnly Property HideBreadCrumbs() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_HIDE_BREADCRUMBS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_HIDE_BREADCRUMBS).ToString())
                Else
                    Return Constants.DEFAULT_HIDE_BREADCRUMBS
                End If
            End Get
        End Property

        Public ReadOnly Property HidePager() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_HIDE_PAGER)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_HIDE_PAGER).ToString())
                Else
                    Return Constants.DEFAULT_HIDE_PAGER
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeJQuery() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_INCLUDE_JQUERY)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_INCLUDE_JQUERY).ToString())
                Else
                    Return Constants.DEFAULT_INCLUDE_JQUERY
                End If
            End Get
        End Property

        Public ReadOnly Property IncludeViewCart() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_INCLUDE_VIEWCART)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_INCLUDE_VIEWCART).ToString())
                Else
                    Return Constants.DEFAULT_INCLUDE_VIEWCART
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumsPerRow() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_ALBUMS_PER_ROW)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_ALBUMS_PER_ROW).ToString())
                Else
                    Return Constants.DEFAULT_ALBUMS_PER_ROW
                End If
            End Get
        End Property

        Public ReadOnly Property PhotosPerPage() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_PHOTOS_PER_PAGE)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_PHOTOS_PER_PAGE).ToString())
                Else
                    Return Constants.DEFAULT_PHOTOS_PER_PAGE
                End If
            End Get
        End Property

        Public ReadOnly Property PhotosPerRow() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_PHOTOS_PER_ROW)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_PHOTOS_PER_ROW).ToString())
                Else
                    Return Constants.DEFAULT_PHOTOS_PER_ROW
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumSortBy() As Common.AlbumSortType
            Get

                If (_settings.Contains(Constants.SETTING_ALBUM_SORT_BY)) Then
                    Return CType(System.Enum.Parse(GetType(Common.AlbumSortType), _settings(Constants.SETTING_ALBUM_SORT_BY).ToString()), AlbumSortType)
                Else
                    Return CType(System.Enum.Parse(GetType(Common.AlbumSortType), Constants.DEFAULT_ALBUM_SORT_BY), AlbumSortType)
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumSortDirection() As Common.SortDirection
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_SORT_DIRECTION)) Then
                    Return CType(System.Enum.Parse(GetType(Common.SortDirection), _settings(Constants.SETTING_ALBUM_SORT_DIRECTION).ToString()), SortDirection)
                Else
                    Return CType(System.Enum.Parse(GetType(Common.SortDirection), Constants.DEFAULT_ALBUM_SORT_DIRECTION), SortDirection)
                End If
            End Get
        End Property

        Public ReadOnly Property SortBy() As Common.SortType
            Get
                If (_settings.Contains(Constants.SETTING_SORT_BY)) Then
                    Return CType(System.Enum.Parse(GetType(Common.SortType), _settings(Constants.SETTING_SORT_BY).ToString()), SortType)
                Else
                    Return CType(System.Enum.Parse(GetType(Common.SortType), Constants.DEFAULT_SORT_BY), SortType)
                End If
            End Get
        End Property

        Public ReadOnly Property SortDirection() As Common.SortDirection
            Get
                If (_settings.Contains(Constants.SETTING_SORT_DIRECTION)) Then
                    Return CType(System.Enum.Parse(GetType(Common.SortDirection), _settings(Constants.SETTING_SORT_DIRECTION).ToString()), SortDirection)
                Else
                    Return CType(System.Enum.Parse(GetType(Common.SortDirection), Constants.DEFAULT_SORT_DIRECTION), SortDirection)
                End If
            End Get
        End Property

        Public ReadOnly Property AlbumFilter() As String
            Get
                If (_settings.Contains(Constants.SETTING_ALBUM_FILTER)) Then
                    Return _settings(Constants.SETTING_ALBUM_FILTER).ToString()
                Else
                    Return Constants.DEFAULT_ALBUM_FILTER
                End If
            End Get
        End Property

        Public ReadOnly Property BorderStyle() As String
            Get
                If (_settings.Contains(Constants.SETTING_BORDER_STYLE)) Then
                    Return _settings(Constants.SETTING_BORDER_STYLE).ToString()
                Else
                    Return Constants.DEFAULT_BORDER_STYLE
                End If
            End Get
        End Property

        Public ReadOnly Property StandardWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_STANDARD_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_STANDARD_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_STANDARD_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property PopupWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_POPUP_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_POPUP_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_POPUP_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property PopupHeight() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_POPUP_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_POPUP_HEIGHT).ToString())
                Else
                    Return Constants.DEFAULT_POPUP_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property EnableScrollbar() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ENABLE_SCROLLBAR)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ENABLE_SCROLLBAR).ToString())
                Else
                    Return Constants.DEFAULT_ENABLE_SCROLLBAR
                End If
            End Get
        End Property

        Public ReadOnly Property PhotoModeration() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_PHOTO_MODERATION)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_PHOTO_MODERATION).ToString())
                Else
                    Return Constants.DEFAULT_PHOTO_MODERATION
                End If
            End Get
        End Property

        Public ReadOnly Property EnableTags() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ENABLE_TAGS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ENABLE_TAGS).ToString())
                Else
                    Return Constants.DEFAULT_ENABLE_TAGS
                End If
            End Get
        End Property

        Public ReadOnly Property RequireTags() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_REQUIRE_TAGS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_REQUIRE_TAGS).ToString())
                Else
                    Return Constants.DEFAULT_REQUIRE_TAGS
                End If
            End Get
        End Property

        Public ReadOnly Property TagCount() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_TAG_COUNT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_TAG_COUNT).ToString())
                Else
                    Return Constants.DEFAULT_TAG_COUNT
                End If
            End Get
        End Property

        Public ReadOnly Property UseXmpExif() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_USE_XMP_EXIF)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_USE_XMP_EXIF).ToString())
                Else
                    Return Constants.DEFAULT_USE_XMP_EXIF
                End If
            End Get
        End Property

        Public ReadOnly Property ZipEnabled() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ZIP_ENABLED)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ZIP_ENABLED).ToString())
                Else
                    Return Constants.DEFAULT_ZIP_ENABLED
                End If
            End Get
        End Property

        Public ReadOnly Property ZipIncludeSubFolders() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_ZIP_INCLUDE_SUBFOLDERS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_ZIP_INCLUDE_SUBFOLDERS).ToString())
                Else
                    Return Constants.DEFAULT_ZIP_INCLUDE_SUBFOLDERS
                End If
            End Get
        End Property

        Public ReadOnly Property RandomMode() As ModeType
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_MODE)) Then
                    Return CType(System.Enum.Parse(GetType(ModeType), _settings(Constants.SETTING_RANDOM_MODE).ToString()), ModeType)
                Else
                    Return Constants.DEFAULT_RANDOM_MODE
                End If
            End Get
        End Property

        Public ReadOnly Property RandomDisplay() As DisplayType
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_DISPLAY)) Then
                    Return CType(System.Enum.Parse(GetType(DisplayType), _settings(Constants.SETTING_RANDOM_DISPLAY).ToString()), DisplayType)
                Else
                    Return Constants.DEFAULT_RANDOM_DISPLAY
                End If
            End Get
        End Property

        Public ReadOnly Property RandomMaxCount() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_MAX_COUNT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_MAX_COUNT).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_MAX_COUNT
                End If
            End Get
        End Property

        Public ReadOnly Property RandomTagFilter() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_TAG_FILTER)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_TAG_FILTER).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_TAG_FILTER
                End If
            End Get
        End Property

        Public ReadOnly Property RandomRepeatDirection() As System.Web.UI.WebControls.RepeatDirection
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_REPEAT_DIRECTION)) Then
                    Return CType(System.Enum.Parse(GetType(System.Web.UI.WebControls.RepeatDirection), _settings(Constants.SETTING_RANDOM_REPEAT_DIRECTION).ToString()), System.Web.UI.WebControls.RepeatDirection)
                Else
                    Return Constants.DEFAULT_RANDOM_REPEAT_DIRECTION
                End If
            End Get
        End Property

        Public ReadOnly Property RandomRepeatColumns() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_REPEAT_COLUMNS)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_REPEAT_COLUMNS).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_REPEAT_COLUMNS
                End If
            End Get
        End Property

        Public ReadOnly Property RandomCompression() As CompressionType
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_COMPRESSION)) Then
                    Return CType(System.Enum.Parse(GetType(CompressionType), _settings(Constants.SETTING_RANDOM_COMPRESSION).ToString()), CompressionType)
                Else
                    Return Constants.DEFAULT_RANDOM_COMPRESSION
                End If
            End Get
        End Property

        Public ReadOnly Property RandomTemplateMode() As TemplateModeType
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_TEMPLATE_MODE)) Then
                    Return CType(System.Enum.Parse(GetType(TemplateModeType), _settings(Constants.SETTING_RANDOM_TEMPLATE_MODE).ToString()), TemplateModeType)
                Else
                    Return Constants.DEFAULT_RANDOM_TEMPLATE_MODE
                End If
            End Get
        End Property

        Public ReadOnly Property RandomThumbnail() As ThumbnailType
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_THUMBNAIL)) Then
                    Return CType(System.Enum.Parse(GetType(ThumbnailType), _settings(Constants.SETTING_RANDOM_THUMBNAIL).ToString()), ThumbnailType)
                Else
                    Return Constants.DEFAULT_RANDOM_THUMBNAIL
                End If
            End Get
        End Property

        Public ReadOnly Property RandomWidth() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_WIDTH)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_WIDTH).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_WIDTH
                End If
            End Get
        End Property

        Public ReadOnly Property RandomHeight() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_HEIGHT)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_HEIGHT).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_HEIGHT
                End If
            End Get
        End Property

        Public ReadOnly Property RandomSquare() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_SQUARE)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_RANDOM_SQUARE).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_SQUARE
                End If
            End Get
        End Property

        Public ReadOnly Property RandomTemplate() As String
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_TEMPLATE)) Then
                    Return _settings(Constants.SETTING_RANDOM_TEMPLATE).ToString()
                Else
                    Return Constants.DEFAULT_RANDOM_TEMPLATE
                End If
            End Get
        End Property

        Public ReadOnly Property RandomTemplateAlbum() As String
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_TEMPLATE_ALBUM)) Then
                    Return _settings(Constants.SETTING_RANDOM_TEMPLATE_ALBUM).ToString()
                Else
                    Return Constants.DEFAULT_RANDOM_TEMPLATE_ALBUM
                End If
            End Get
        End Property

        Public ReadOnly Property RandomLaunchSlideshow() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_LAUNCH_SLIDESHOW)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_RANDOM_LAUNCH_SLIDESHOW).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_LAUNCH_SLIDESHOW
                End If
            End Get
        End Property

        Public ReadOnly Property RandomAlbumSlideshow() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_ALBUM_SLIDESHOW)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_RANDOM_ALBUM_SLIDESHOW).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_ALBUM_SLIDESHOW
                End If
            End Get
        End Property

        Public ReadOnly Property RandomIncludeJQuery() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_RANDOM_INCLUDE_JQUERY)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_RANDOM_INCLUDE_JQUERY).ToString())
                Else
                    Return Constants.DEFAULT_RANDOM_INCLUDE_JQUERY
                End If
            End Get
        End Property

        Public ReadOnly Property SearchTemplate() As String
            Get
                If (_settings.Contains(Constants.SETTING_SEARCH_TEMPLATE)) Then
                    Return _settings(Constants.SETTING_SEARCH_TEMPLATE).ToString()
                Else
                    Return Constants.DEFAULT_SEARCH_TEMPLATE
                End If
            End Get
        End Property

        Public ReadOnly Property UploaderFileSize() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_UPLOADER_FILE_SIZE)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_UPLOADER_FILE_SIZE).ToString())
                Else
                    Return Constants.DEFAULT_UPLOADER_FILE_SIZE
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxNextKey() As String
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_NEXT_KEY)) Then
                    Return _settings(Constants.SETTING_LIGHTBOX_NEXT_KEY).ToString()
                Else
                    Return Constants.DEFAULT_LIGHTBOX_NEXT_KEY
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxPreviousKey() As String
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_PREVIOUS_KEY)) Then
                    Return _settings(Constants.SETTING_LIGHTBOX_PREVIOUS_KEY).ToString()
                Else
                    Return Constants.DEFAULT_LIGHTBOX_PREVIOUS_KEY
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxCloseKey() As String
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_CLOSE_KEY)) Then
                    Return _settings(Constants.SETTING_LIGHTBOX_CLOSE_KEY).ToString()
                Else
                    Return Constants.DEFAULT_LIGHTBOX_CLOSE_KEY
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxDownloadKey() As String
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_DOWNLOAD_KEY)) Then
                    Return _settings(Constants.SETTING_LIGHTBOX_DOWNLOAD_KEY).ToString()
                Else
                    Return Constants.DEFAULT_LIGHTBOX_DOWNLOAD_KEY
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxSlideInterval() As Integer
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_SLIDE_INTERVAL)) Then
                    Return Convert.ToInt32(_settings(Constants.SETTING_LIGHTBOX_SLIDE_INTERVAL).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_SLIDE_INTERVAL
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxHideTitle() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_HIDE_TITLE)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_LIGHTBOX_HIDE_TITLE).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_HIDE_TITLE
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxHideDescription() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_HIDE_DESCRIPTION)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_LIGHTBOX_HIDE_DESCRIPTION).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_HIDE_DESCRIPTION
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxHidePaging() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_HIDE_PAGING)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_LIGHTBOX_HIDE_PAGING).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_HIDE_PAGING
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxHideTags() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_HIDE_TAGS)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_LIGHTBOX_HIDE_TAGS).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_HIDE_TAGS
                End If
            End Get
        End Property

        Public ReadOnly Property LightboxHideDownload() As Boolean
            Get
                If (_settings.Contains(Constants.SETTING_LIGHTBOX_HIDE_DOWNLOAD)) Then
                    Return Convert.ToBoolean(_settings(Constants.SETTING_LIGHTBOX_HIDE_DOWNLOAD).ToString())
                Else
                    Return Constants.DEFAULT_LIGHTBOX_HIDE_DOWNLOAD
                End If
            End Get
        End Property

#End Region

    End Class

End Namespace


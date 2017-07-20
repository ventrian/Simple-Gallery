'
' Simple Gallery for DotNetNuke -  http://www.dotnetnuke.com
' Copyright (c) 2002-2007
' by Ventrian ( sales@ventrian.com ) ( http://www.ventrian.com )
'

Imports DotNetNuke.Common.Utilities

Namespace Ventrian.SimpleGallery.Common

    Public Class Constants

        Public Const JAVASCRIPT_VERSION As String = "020426"

        Public Const DEFAULT_ALBUM_DEFAULT_PATH As String = "Gallery/Album/[AlbumID]"
        Public Const DEFAULT_LIGHTBOX_DEFAULT_PATH As String = "~/desktopmodules/simplegallery/js/lightbox/jquery.lightbox-0.4.pack.js"
        Public Const DEFAULT_RESIZE_PHOTO As Boolean = True
        Public Const DEFAULT_COMPRESSION As CompressionType = CompressionType.Quality
        Public Const DEFAULT_COMPRESSION_ALBUM As CompressionType = CompressionType.Quality
        Public Const DEFAULT_COMPRESSION_PHOTO As CompressionType = CompressionType.Quality
        Public Const DEFAULT_THUMBNAIL_ALBUM As ThumbnailType = ThumbnailType.Proportion
        Public Const DEFAULT_THUMBNAIL_PHOTO As ThumbnailType = ThumbnailType.Proportion
        Public Const DEFAULT_WIDTH As Integer = 600
        Public Const DEFAULT_HEIGHT As Integer = 480
        Public Const DEFAULT_ALBUM_THUMBNAIL_WIDTH As Integer = 250
        Public Const DEFAULT_ALBUM_THUMBNAIL_HEIGHT As Integer = 250
        Public Const DEFAULT_ALBUM_THUMBNAIL_SQUARE As Integer = 250
        Public Const DEFAULT_THUMBNAIL_WIDTH As Integer = 125
        Public Const DEFAULT_THUMBNAIL_HEIGHT As Integer = 125
        Public Const DEFAULT_THUMBNAIL_SQUARE As Integer = 125
        Public Const DEFAULT_USE_WATERMARK As Boolean = False
        Public Const DEFAULT_USE_ALBUM_ANCHORS As Boolean = False
        Public Const DEFAULT_WATERMARK_TEXT As String = ""
        Public Const DEFAULT_WATERMARK_IMAGE As String = ""
        Public Const DEFAULT_WATERMARK_IMAGE_POSITION As WatermarkPosition = WatermarkPosition.BottomRight
        Public Const DEFAULT_ENABLE_SEARCH As Boolean = True
        Public Const DEFAULT_ENABLE_SYNDICATION As Boolean = True
        Public Const DEFAULT_ENABLE_TOOLTIP As Boolean = True
        Public Const DEFAULT_HIDE_BREADCRUMBS As Boolean = False
        Public Const DEFAULT_HIDE_PAGER As Boolean = False
        Public Const DEFAULT_INCLUDE_JQUERY As Boolean = True
        Public Const DEFAULT_INCLUDE_VIEWCART As Boolean = False
        Public Const DEFAULT_PHOTOS_PER_PAGE As Integer = 16
        Public Const DEFAULT_PHOTOS_PER_ROW As Integer = 4
        Public Const DEFAULT_ALBUMS_PER_ROW As Integer = 2
        Public Const DEFAULT_ALBUM_SORT_BY As String = "Caption"
        Public Const DEFAULT_ALBUM_SORT_DIRECTION As String = "ASC"
        Public Const DEFAULT_SORT_BY As String = "Name"
        Public Const DEFAULT_SORT_DIRECTION As String = "ASC"
        Public Const DEFAULT_ALBUM_FILTER As String = "-1"
        Public Const DEFAULT_BORDER_STYLE As String = "White"
        Public Const DEFAULT_STANDARD_WIDTH As Integer = 700
        Public Const DEFAULT_POPUP_WIDTH As Integer = 645
        Public Const DEFAULT_POPUP_HEIGHT As Integer = 560
        Public Const DEFAULT_ENABLE_SCROLLBAR As Boolean = False
        Public Const DEFAULT_PHOTO_MODERATION As Boolean = False
        Public Const DEFAULT_ENABLE_TAGS As Boolean = True
        Public Const DEFAULT_REQUIRE_TAGS As Boolean = False
        Public Const DEFAULT_TAG_COUNT As Integer = 60
        Public Const DEFAULT_SLIDESHOW_TYPE As SlideshowType = SlideshowType.Lightbox
        Public Const DEFAULT_UPLOADER_FILE_SIZE As Integer = 4096
        Public Const DEFAULT_USE_XMP_EXIF As Boolean = True
        Public Const DEFAULT_ZIP_ENABLED As Boolean = False
        Public Const DEFAULT_ZIP_INCLUDE_SUBFOLDERS As Boolean = True
        Public Const DEFAULT_LIGHTBOX_NEXT_KEY As String = "n"
        Public Const DEFAULT_LIGHTBOX_PREVIOUS_KEY As String = "p"
        Public Const DEFAULT_LIGHTBOX_CLOSE_KEY As String = "c"
        Public Const DEFAULT_LIGHTBOX_DOWNLOAD_KEY As String = "d"
        Public Const DEFAULT_LIGHTBOX_SLIDE_INTERVAL As Integer = 5
        Public Const DEFAULT_LIGHTBOX_HIDE_TITLE As Boolean = False
        Public Const DEFAULT_LIGHTBOX_HIDE_DESCRIPTION As Boolean = False
        Public Const DEFAULT_LIGHTBOX_HIDE_PAGING As Boolean = False
        Public Const DEFAULT_LIGHTBOX_HIDE_TAGS As Boolean = False
        Public Const DEFAULT_LIGHTBOX_HIDE_DOWNLOAD As Boolean = False

        Public Const DEFAULT_TEMPLATE_ALBUM_INFO As String = "" _
            & "[ALBUMWITHBORDER]" _
            & "[EDIT]<span class=""NormalBold""><a href=""[ALBUMLINK]"">[TITLE]</a></span> [RSS] [ZIP]<span class=""Normal"">[PHOTOCOUNT]</span>" _
            & "<br><span class=""Normal"">[ALBUMCOUNT]</span>"

        Public Const DEFAULT_TEMPLATE_PHOTO_INFO As String = "" _
            & "[PHOTOWITHBORDER]" _
            & "[EDIT]<span class=""Normal"">[TITLE]</span>"

        Public Const SETTING_ALBUM_DEFAULT_PATH As String = "DefaultAlbumPath"
        Public Const SETTING_LIGHTBOX_DEFAULT_PATH As String = "DefaultLightboxPath"
        Public Const SETTING_RESIZE_PHOTO As String = "ResizePhoto"
        Public Const SETTING_COMPRESSION As String = "Compression"
        Public Const SETTING_COMPRESSION_ALBUM As String = "CompressionAlbum"
        Public Const SETTING_COMPRESSION_PHOTO As String = "CompressionPhoto"
        Public Const SETTING_THUMBNAIL_ALBUM As String = "ThumbnailAlbum"
        Public Const SETTING_THUMBNAIL_PHOTO As String = "ThumbnailPhoto"
        Public Const SETTING_WIDTH As String = "Width"
        Public Const SETTING_HEIGHT As String = "Height"
        Public Const SETTING_ALBUM_THUMBNAIL_WIDTH As String = "AlbumThumbnailWidth"
        Public Const SETTING_ALBUM_THUMBNAIL_HEIGHT As String = "AlbumThumbnailHeight"
        Public Const SETTING_ALBUM_THUMBNAIL_SQUARE As String = "AlbumThumbnailSquare"
        Public Const SETTING_THUMBNAIL_WIDTH As String = "ThumbnailWidth"
        Public Const SETTING_THUMBNAIL_HEIGHT As String = "ThumbnailHeight"
        Public Const SETTING_THUMBNAIL_SQUARE As String = "ThumbnailSquare"
        Public Const SETTING_USE_WATERMARK As String = "UseWatermark"
        Public Const SETTING_USE_ALBUM_ANCHORS As String = "UseAlbumAnchors"
        Public Const SETTING_WATERMARK_TEXT As String = "WatermarkText"
        Public Const SETTING_WATERMARK_IMAGE As String = "WatermarkImage"
        Public Const SETTING_WATERMARK_IMAGE_POSITION As String = "WatermarkImagePosition"
        Public Const SETTING_ENABLE_SEARCH As String = "EnableSearch"
        Public Const SETTING_ENABLE_SYNDICATION As String = "EnableSyndication"
        Public Const SETTING_ENABLE_TOOLTIP As String = "EnableTooltip"
        Public Const SETTING_HIDE_BREADCRUMBS As String = "HideBreadCrumbs"
        Public Const SETTING_HIDE_PAGER As String = "HidePager"
        Public Const SETTING_INCLUDE_JQUERY As String = "IncludeJQuery"
        Public Const SETTING_INCLUDE_VIEWCART As String = "IncludeViewCart"
        Public Const SETTING_PHOTOS_PER_PAGE As String = "PhotosPerPage"
        Public Const SETTING_PHOTOS_PER_ROW As String = "PhotosPerRow"
        Public Const SETTING_ALBUMS_PER_ROW As String = "AlbumsPerRow"
        Public Const SETTING_ALBUM_SORT_BY As String = "AlbumSortBy"
        Public Const SETTING_ALBUM_SORT_DIRECTION As String = "AlbumSortDirection"
        Public Const SETTING_SORT_BY As String = "SortBy"
        Public Const SETTING_SORT_DIRECTION As String = "SortDirection"
        Public Const SETTING_ALBUM_FILTER As String = "AlbumFilter"
        Public Const SETTING_BORDER_STYLE As String = "BorderStyle"
        Public Const SETTING_SLIDESHOW_TYPE As String = "SlideshowType"
        Public Const SETTING_STANDARD_WIDTH As String = "StandardWidth"
        Public Const SETTING_USE_POPUP_WINDOW As String = "UsePopupWindow"
        Public Const SETTING_POPUP_WIDTH As String = "PopupWidth"
        Public Const SETTING_POPUP_HEIGHT As String = "PopupHeight"
        Public Const SETTING_ENABLE_SCROLLBAR As String = "EnableScrollbar"
        Public Const SETTING_PHOTO_MODERATION As String = "PhotoModeration"
        Public Const SETTING_ENABLE_TAGS As String = "EnableTags"
        Public Const SETTING_REQUIRE_TAGS As String = "RequireTags"
        Public Const SETTING_TAG_COUNT As String = "TagCount"
        Public Const SETTING_UPLOADER_FILE_SIZE As String = "UploaderFileSize"
        Public Const SETTING_USE_XMP_EXIF As String = "UseXmpExif"
        Public Const SETTING_ZIP_ENABLED As String = "ZipEnabled"
        Public Const SETTING_ZIP_INCLUDE_SUBFOLDERS As String = "ZipIncludeSubFolders"
        Public Const SETTING_LIGHTBOX_NEXT_KEY As String = "LightboxNextKey"
        Public Const SETTING_LIGHTBOX_PREVIOUS_KEY As String = "LightboxPreviousKey"
        Public Const SETTING_LIGHTBOX_CLOSE_KEY As String = "LightboxCloseKey"
        Public Const SETTING_LIGHTBOX_DOWNLOAD_KEY As String = "LightboxDownloadKey"
        Public Const SETTING_LIGHTBOX_SLIDE_INTERVAL As String = "LightboxSlideInterval"
        Public Const SETTING_LIGHTBOX_HIDE_TITLE As String = "LightboxHideTitle"
        Public Const SETTING_LIGHTBOX_HIDE_DESCRIPTION As String = "LightboxHideDescription"
        Public Const SETTING_LIGHTBOX_HIDE_PAGING As String = "LightboxHidePaging"
        Public Const SETTING_LIGHTBOX_HIDE_TAGS As String = "LightboxHideTags"
        Public Const SETTING_LIGHTBOX_HIDE_DOWNLOAD As String = "LightboxHideDownload"

        Public Const SETTING_PERMISSION_ADD_ALBUM As String = "PermissionAddAlbum"
        Public Const SETTING_PERMISSION_EDIT_ALBUM As String = "PermissionEditAlbum"
        Public Const SETTING_PERMISSION_DELETE_ALBUM As String = "PermissionDeleteAlbum"

        ' Security Settings
        Public Const SETTING_UPLOAD_ROLES As String = "UploadRoles"
        Public Const SETTING_EDIT_ROLES As String = "EditRoles"
        Public Const SETTING_DELETE_ROLES As String = "DeleteRoles"
        Public Const SETTING_APPROVE_ROLES As String = "ApproveRoles"
        Public Const SETTING_ALBUM_ROLES As String = "AlbumRoles"

        Public Const SETTING_RANDOM_MODE As String = "RandomMode"
        Public Const SETTING_RANDOM_DISPLAY As String = "RandomDisplay"
        Public Const SETTING_RANDOM_COMPRESSION As String = "RandomCompression"
        Public Const SETTING_RANDOM_THUMBNAIL As String = "RandomThumbnail"
        Public Const SETTING_RANDOM_TEMPLATE_MODE As String = "RandomTemplateMode"
        Public Const SETTING_RANDOM_MAX_COUNT As String = "RandomMaxCount"
        Public Const SETTING_RANDOM_TAG_FILTER As String = "RandomTagFilter"
        Public Const SETTING_RANDOM_REPEAT_DIRECTION As String = "RandomRepeatDirection"
        Public Const SETTING_RANDOM_REPEAT_COLUMNS As String = "RandomRepeatColumns"

        Public Const SETTING_RANDOM_PHOTO_TAB_ID As String = "RandomTabID"
        Public Const SETTING_RANDOM_PHOTO_MODULE_ID As String = "RandomModuleID"
        Public Const SETTING_RANDOM_PHOTO_ALBUM_ID As String = "RandomAlbumID"
        Public Const SETTING_RANDOM_WIDTH As String = "RandomWidth"
        Public Const SETTING_RANDOM_HEIGHT As String = "Randomheight"
        Public Const SETTING_RANDOM_SQUARE As String = "RandomSquare"
        Public Const SETTING_RANDOM_TEMPLATE As String = "RandomTemplate"
        Public Const SETTING_RANDOM_TEMPLATE_ALBUM As String = "RandomTemplateAlbum"
        Public Const SETTING_RANDOM_LAUNCH_SLIDESHOW As String = "RandomLaunchSlideshow"
        Public Const SETTING_RANDOM_ALBUM_SLIDESHOW As String = "RandomAlbumSlideshow"
        Public Const SETTING_RANDOM_INCLUDE_JQUERY As String = "RandomIncludeJQuery"

        Public Const DEFAULT_RANDOM_MODE As ModeType = ModeType.Latest
        Public Const DEFAULT_RANDOM_DISPLAY As DisplayType = DisplayType.Photo
        Public Const DEFAULT_RANDOM_COMPRESSION As CompressionType = CompressionType.Quality
        Public Const DEFAULT_RANDOM_THUMBNAIL As ThumbnailType = ThumbnailType.Proportion
        Public Const DEFAULT_RANDOM_TEMPLATE_MODE As TemplateModeType = TemplateModeType.Simple
        Public Const DEFAULT_RANDOM_MAX_COUNT As Integer = 5
        Public Const DEFAULT_RANDOM_TAG_FILTER As Integer = -1
        Public Const DEFAULT_RANDOM_REPEAT_DIRECTION As System.Web.UI.WebControls.RepeatDirection = Web.UI.WebControls.RepeatDirection.Vertical
        Public Const DEFAULT_RANDOM_REPEAT_COLUMNS As Integer = 1
        Public Const DEFAULT_RANDOM_WIDTH As Integer = 200
        Public Const DEFAULT_RANDOM_HEIGHT As Integer = 200
        Public Const DEFAULT_RANDOM_SQUARE As Integer = 200
        Public Const DEFAULT_RANDOM_TEMPLATE As String = "[PHOTOWITHBORDER]"
        Public Const DEFAULT_RANDOM_TEMPLATE_ALBUM As String = "[ALBUMWITHBORDER]"
        Public Const DEFAULT_RANDOM_LAUNCH_SLIDESHOW As Boolean = True
        Public Const DEFAULT_RANDOM_ALBUM_SLIDESHOW As Boolean = False
        Public Const DEFAULT_RANDOM_INCLUDE_JQUERY As Boolean = True

        Public Const SETTING_SEARCH_TAB_ID As String = "SearchTabID"
        Public Const SETTING_SEARCH_MODULE_ID As String = "SearchModuleID"
        Public Const SETTING_SEARCH_TAB_MODULE_ID As String = "SearchTabModuleID"
        Public Const SETTING_SEARCH_TEMPLATE As String = "SearchTemplate"
        Public Const DEFAULT_SEARCH_TEMPLATE As String = "[TEXTBOX][BUTTON]"

        Public Const SETTING_TAG_CLOUD_PHOTO_TAB_ID As String = "TagCloudTabID"
        Public Const SETTING_TAG_CLOUD_PHOTO_MODULE_ID As String = "TagCloudModuleID"
        Public Const SETTING_TAG_CLOUD_PHOTO_TAB_MODULE_ID As String = "TagCloudTabModuleID"
        Public Const SETTING_TAG_CLOUD_PHOTO_ALBUM_ID As String = "TagCloudAlbumID"
        Public Const SETTING_TAG_CLOUD_MAX_COUNT As String = "RandomMaxCount"

        Public Const DEFAULT_TAG_CLOUD_MAX_COUNT As Integer = 60

    End Class

End Namespace

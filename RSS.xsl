<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:dc="http://purl.org/dc/elements/1.1/" version="1.0">
	<xsl:output method="xml"  />
	<xsl:template match="/">

		<html xmlns="http://www.w3.org/1999/xhtml" lang="en" xml:lang="en">
			<head>
				<title>
					<xsl:value-of select="rss/channel/title"/>
				</title>
				<style>

					body {
					margin: 0px;
					padding: 0px;
					color: #000000;
					font-family: Arial, Helvetica;
					font-size: 9pt;
					background-color: #ffffff;
					color: #000000;
					}

					a:LINK {
					color: #666699;
					text-decoration: none;
					}

					a:ACTIVE {
					color: #ff0000;
					}

					a:VISITED {
					color: #000066;
					text-decoration: none;
					}

					a:HOVER {
					text-decoration: underline;
					}

					#Content {
					padding-top: 0px;
					padding-left: 35px;
					padding-right: 35px;
					}

					.rss {
					position: relative;
					display: inline;
					background-color: orange;
					color: #ffffff;
					border: solid 2px black;
					padding: 5px;
					padding-top: 2px;
					padding-bottom: 2px;
					font-weight: bold;
					font-family: Arial, Helvetica;
					margin: 0px;
					font-size: 25pt;
					left: -15px;
					top: -5px;
					}

					h1,h2,h4 {
					color: #666666;
					font-weight: bold;
					font-family: Arial, Arial, Helvetica;
					margin: 0px;
					font-size: 20pt;
					align: centre;
					}

					h2 {
					font-size: 16pt;
					margin-left: 16px;
					}

					h4 {
					font-size: 11pt;
					font-family: Arial, Helvetica;
					}

					#ContentDescription {
					margin: 35px;
					margin-bottom: 10px;
					color: #000000;
					background-color: #dddddd;
					padding-left: 5px;
					padding-right: 5px;
					padding-bottom: 5px;
					}

					.ItemListItem {
					padding-bottom: 8px;
					}

					.ItemListItemDetails {
					font-family: Arial, Helvetica;
					font-size: 8pt;
					color: #333333;
					}


					.photo-frame .topx--
					{
					background-repeat: no-repeat;
					width: auto;
					height: 4px;
					vertical-align: top;
					background-image: url("images/borders/white/frame-topx--.gif");
					}
					.photo-frame .top-x- {
					background-repeat: repeat-x;
					width: auto;
					height: 4px;
					background-image: url("images/borders/white/frame-top-x-.gif");
					}
					.photo-frame .top--x {
					background-repeat: no-repeat;
					width: auto;
					height: 4px;
					vertical-align: top;
					background-image: url("images/borders/white/frame-top--x.gif");
					}

					.photo-frame .midx-- {
					background-repeat: repeat-y;
					width: 4px;
					height: auto;
					background-image: url("images/borders/white/frame-midx--.gif");
					}
					.photo-frame .mid--x {
					background-repeat: repeat-y;
					width: 4px;
					height: auto;
					background-image: url("images/borders/white/frame-mid--x.gif");
					}

					.photo-frame .botx-- {
					background-repeat: no-repeat;
					width: 4px;
					height: 4px;
					vertical-align: top;
					background-image: url("images/borders/white/frame-botx--.gif");
					}
					.photo-frame .bot-x- {
					background-repeat: repeat-x;
					width: auto;
					height: 4px;
					background-image: url("images/borders/white/frame-bot-x-.gif");
					}
					.photo-frame .bot--x {
					background-repeat: no-repeat;
					width: 4px;
					height: 4px;
					vertical-align: bottom;
					background-image: url("images/borders/white/frame-bot--x.gif");
					}

					.photo_198 {
					border: 4px solid #FFFFFF;
					}

				</style>
			</head>
			<body xmlns="http://www.w3.org/1999/xhtml">
				<div id="ContentDescription">
					<div class="rss">RSS</div>
					<p>
						The resource you requested is known as an RSS syndication feed. RSS is an acronym for Rich Site Summary and is a document format for distributing and
						syndicating content that allows the content to be easily reused.
					</p>
					<p>
						Using this web address (URL) you can consume this content through a variety of tools and websites. For individuals, the most likely usage
						is within an application known as an RSS reader. An RSS reader is an application you run on your own computer which can request and store the
						content from RSS feeds.
					</p>
					<p>
						To use this feed in your RSS reader software you will need the web address (URL) of this page. If you wish to view the XML generated by this RSS feed, please select View | Source from within your web browser.
					</p>
				</div>

				<div id="Content">
					<h1>
						<xsl:value-of select="rss/channel/title"/>
					</h1>
					<br />

					<xsl:for-each select="rss/channel/item">
						<h4>
							<a>
								<xsl:attribute name="href">
									<xsl:value-of select="link"/>
								</xsl:attribute>
								<xsl:value-of select="title"/>
							</a>
						</h4>
						<table border="0" cellpadding="0" cellspacing="0" class="photo-frame">
							<tr>
								<td class="topx--"></td>
								<td class="top-x-"></td>
								<td class="top--x"></td>
							</tr>
							<tr>
								<td class="midx--"></td>
								<td>
									<a>
										<xsl:attribute name="href">
											<xsl:value-of select="link"/>
										</xsl:attribute>
										<img class="photo_198" border="0">
											<xsl:attribute name="src">
												<xsl:value-of select="linkThumbnail"/>
											</xsl:attribute>
										</img>
									</a>
								</td>
								<td class="mid--x"></td>
							</tr>
							<tr>
								<td class="botx--"></td>
								<td class="bot-x-"></td>
								<td class="bot--x"></td>
							</tr>
						</table>
						<div class="ItemListItemDetails">
							Published <xsl:value-of select="pubDate"/>
							by <xsl:value-of select="dc:creator" />
						</div>
						<hr />
					</xsl:for-each>
				</div>

			</body>
		</html>

	</xsl:template>
</xsl:stylesheet>
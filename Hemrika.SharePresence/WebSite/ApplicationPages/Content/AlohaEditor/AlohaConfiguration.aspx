<%@ Assembly Name="Hemrika.SharePresence.WebSite, Version=1.0.0.0, Culture=neutral, PublicKeyToken=3421bd1d946bda6c" %>
<%@ Page Language="C#" AutoEventWireup="false" EnableViewState="false" Inherits="Hemrika.SharePresence.WebSite.AlohaConfiguration" %>
function HTML5EditorEnable() {

    //var Aloha = window.Aloha || ( window.Aloha = {} );
    Aloha = window.Aloha || {};

    Aloha.settings = {
        logLevels: { 'error': true, 'warn': true, 'info': true, 'debug': true, 'deprecated': true },
        errorhandling: true,
        baseUrl: "/_layouts/Hemrika/Editor/lib",
        basePath: "/_layouts/Hemrika/Editor/plugins/",
        locale: 'nl',
        ribbon: false,
        sidebar: { disabled: true },
        plugins: {
            format: {
                // all elements with no specific configuration get this configuration
                config: [ 'strong', 'em', 'b', 'i', 's', 'p', 'sub', 'sup', 'del', 'title', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6', 'pre', 'removeFormat' ],
                editables: {
                    // no formatting allowed for title
                    '.Title': [],
                    '.Image': []
                }
            },
            block: {
                rootTags : [ 'span', 'div' , 'section' , 'header' , 'footer' , 'article', 'aside' ]
            },

            /*
            list: {
                // all elements with no specific configuration get an UL, just for fun :)
                config: ['ul', 'ol'],
                editables: {
                    // Even if this is configured it is not set because OL and UL are not allowed in H1.
                    '.Title': []
                }
            },
            */
            
            /*
            listenforcer: {
                editables: ['.aloha-enforce-lists']
            },
            */
            
            metaview: {
                config:['enabled'],
                editables: {
                    '.Title': [ 'disable' ],
                    '.Image': [ 'disable' ]
                    }
            },
            /*
            abbr: {
            // all elements with no specific configuration get an UL, just for fun :)
            config: ['abbr'],
            editables: {
            // Even if this is configured it is not set because OL and UL are not allowed in H1.
            '#top-text': []
                }
            },
            */

            link: {
                // all elements with no specific configuration may insert links
                config: ['a'],
                hotKey: {
                    // use ctrl+l instead of ctrl+k as hotkey for inserting a link
                    //insertLink: 'ctrl+l'
                },
                editables: {
                    // Nothing in the title.
                    '.Title': [],
                    '.Image': [],
                },
                // all links that match the targetregex will get set the target
                // e.g. ^(?!.*aloha-editor.com).* matches all href except aloha-editor.com
                //targetregex: '^(?!.*aloha-editor.com).*',
                // this target is set when either targetregex matches or not set
                // e.g. _blank opens all links in new window
                target: '_self',
                // the same for css class as for target
                //cssclassregex: '^(?!.*aloha-editor.com).*',
                //cssclass: 'aloha',
                // use all resources of type website for autosuggest
                objectTypeFilter: ['website'],
                // handle change of href
                onHrefChange: function (obj, href, item) {
                    var jQuery = Aloha.require('jquery');
                    if (item) {
                        jQuery(obj).attr('data-name', item.name);
                    } else {
                        jQuery(obj).removeAttr('data-name');
                    }
                }
            },
            table: {
                // all elements with no specific configuration are not allowed to insert tables
                config: ['table'],
                editables: {
                    // Don't allow tables in top-text
                    '.Title': [],
                    '.Image': [],
                },
                //summaryinsidebar: true,
                // [{name:'green', text:'Green', tooltip:'Green is cool', iconClass:'GENTICS_table GENTICS_button_green', cssClass:'green'}]
                tableConfig: [
					{ name: 'hor-minimalist-a' },
					{ name: 'box-table-a' },
					{ name: 'hor-zebra' },
				],
                columnConfig: [
					{ name: 'table-style-bigbold', iconClass: 'aloha-button-col-bigbold' },
					{ name: 'table-style-redwhite', iconClass: 'aloha-button-col-redwhite' }
				],
                rowConfig: [
					{ name: 'table-style-bigbold', iconClass: 'aloha-button-row-bigbold' },
					{ name: 'table-style-redwhite', iconClass: 'aloha-button-row-redwhite' }
				],
                cellConfig: [
					{ name: 'table-style-bigbold', iconClass: 'aloha-button-row-bigbold' },
					{ name: 'table-style-redwhite', iconClass: 'aloha-button-row-redwhite' }
				]
            },
            /*
            image: {
                'fixedAspectRatio': true,
                'globalselector': '.Image',
                    'ui': {
                    'oneTab': true,
                    'align': false,
                    'margin': false
                    }
            },
            */
            cite: {
                referenceContainer: '#references'
            },
            formatlesspaste: {
                formatlessPasteOption: true,
                strippedElements: [
				"em",
				"strong",
				"small",
				"s",
				"cite",
				"q",
				"dfn",
				"abbr",
				"time",
				"code",
				"var",
				"samp",
				"kbd",
				"sub",
				"sup",
				"i",
				"b",
				"u",
				"mark",
				"ruby",
				"rt",
				"rp",
				"bdi",
				"bdo",
				"ins",
				"del"]
            },
            'numerated-headers': {
                config: {
                    // default true
                    // numeratedactive will also accept "true" and "1" as true values
                    // false and "false" for false
                    numeratedactive: false,
                    // if the headingselector is empty, the button will not be shown at all
                    headingselector: 'h1, h2, h3, h4, h5, h6', // default: all
                    baseobjectSelector: 'body'                 // if not set: Aloha.activeEditable
                }
            }
        },
        /*
        toolbar: {
            tabs: [
			            {
			                label: 'tab.format.label'
			            },
			            {
			                label: 'tab.insert.label'
			            },
			            {
                            label: 'Actions'
                        },
                            components: [['htmlsource']]
			            },
			            {
			                label: 'Validate'
			            },
		            ]
        },
        */
        contentHandler: {
            insertHtml: [ 'word', 'generic', 'oembed', 'sanitize', 'blockelement' ],
            initEditable: [ 'sanitize' ],
            sanitize: 'relaxed',
    	    allows: {
			    elements: [
				    'a', 'abbr', 'b', 'blockquote', 'br', 'caption', 'cite', 'code', 'col',
				    'colgroup', 'dd', 'dl', 'dt', 'em', 'h1', 'h2', 'h3', 'h4', 'h5', 'h6',
				    'i', 'img', 'li', 'ol', 'p', 'pre', 'q', 'small', 'strike', 'strong',
				    'sub', 'sup', 'table', 'tbody', 'td', 'tfoot', 'th', 'thead', 'tr', 'u',
				    'ul', 'span', 'hr', 'object', 'div', 'script', 'figure', 'noscript', 'figcaption',
				    'iframe', 'param', 'embed', 'summery', 'details'
			    ],
			    attributes: {
				    'a': ['href', 'title', 'id', 'data-gentics-aloha-repository', 'data-gentics-aloha-object-id', 'data-url', 'style', 'class','target', 'name'],
                    'p': ['class', 'style'],
                    'span': ['class', 'style'],
				    'div': [ 'id', 'class', 'style'],
				    'abbr': ['title'],
				    'blockquote': ['cite'],
				    'br': ['class'],
				    'col': ['span', 'width'],
				    'colgroup': ['span', 'width'],
				    'img': ['align', 'alt', 'height', 'src', 'title', 'width', 'data-url', 'class', 'style'],
				    'ol': ['start', 'type'],
				    'q': ['cite'],
                    'script': ['type', 'language', 'src'],
				    'table': ['summary', 'width', 'class','style'],
				    'td': ['abbr', 'axis', 'colspan', 'rowspan', 'width', 'class','style'],
				    'th': ['abbr', 'axis', 'colspan', 'rowspan', 'scope', 'width', 'class','style'],
				    'tr': ['class','style'],
				    'ul': ['type'],
                    'li': ['class', 'style'],
                    'figure': ['data-media','data-media400', 'data-media600', 'title'],
				    'iframe': ['src','width','height','frameborder','marginwidth','marginheight','scrolling','style','allowfullscreen'],
				    'strong': ['style'],
                    'object': ['height','width'],
                    'param': ['name','value'],
                    'embed': ['allowscriptaccess','height','src','type','width'],
                    'details': ['summery']
			    },
			    protocols: {
				    'a': {'href': ['ftp', 'http', 'https', 'mailto', '__relative__']},
				    'blockquote': {'cite': ['http', 'https', '__relative__']},
				    'img': {'src' : ['http', 'https', '__relative__']},
				    'q': {'cite': ['http', 'https', '__relative__']},
				    'script': {'src' : ['http','https','__relative__']}
			    }
		    }
        }
    }
};

function HTML5Editor() {
    if (_WebSitePageContextInfo.wspFormMode == "Edit") {
        Aloha.jQuery('.HTML5-editable').mahalo();

        jQuery('.HTML5-editable').each(function () {
            var content = this.innerHTML;
            var contentId = this.id;
            
            var hidden = document.getElementById(contentId + '_hidden');
            //Going for convention over code, hidden input must be nearby.
            if(!hidden)
            {
                
                hidden = $(this).prev();
                if(!hidden)
                {
                    hidden = $(this).next();
                }

                if(hidden)
                {
                    var s = hidden[0].id
                    contentId = s.slice(0, s.lastIndexOf("_"));
                }

                /*
                var childs = this.childNodes;
                for (var i = 0; i <= childs.length - 1; i++)
                {
                    contentId = childs[i].id;
                    hidden = document.getElementById(contentId + '_hidden');
                    if(hidden) {break;}
                }
                */
                //contentId = this.children()[0].id;
                //hidden = document.getElementById(contentId + '_hidden');
            }

            if(hidden)
            {
                $('#' + contentId + '_hidden').val(content);
            }
        });

    }
};


$(document).ready(function () {
    if (_WebSitePageContextInfo.wspFormMode == "Edit") {
        HTML5EditorEnable();
        Aloha.ready(function () {
        Aloha.jQuery('.HTML5-editable').aloha();
        });
    };
});
/*global Aloha, GENTICS, CodeMirror */
define(function (require) {

	'use strict';

	/**
	 * Module dependencies.
	 */
	var Ui = require('ui/ui'),
		Button = require('ui/button'),
		Dialog = require('ui/dialog'),
		plugin = require('aloha/plugin'),
		htmlBeautifier = require('./htmlbeautifier'),
		codeMirror = require('./codemirror'),
		$ = require('jquery');

	/**
	 * Plugin CSS dependencies.
	 */
	require('css!htmlsource/css/htmlsource');
	require('css!htmlsource/css/codemirror');

	/**
	 * Create & register the plugin.
	 */
	return plugin.create('htmlsource', {

		/**
		 * Dialog width.
		 * @type {Number}
		 */
		width: 600,

		/**
		 * Dialog height.
		 * @type {Number}
		 */
		height: 370,

		/**
		 * The active editor.
		 * @type {Editable}
		 */
		editable: null,

		/**
		 * CodeMirror editor instance.
		 * @type {CodeMirror}
		 */
		editor: null,

		/**
		 * Dialog element.
		 * @type {jQuery.Element}
		 */
		$dialog: null,

		/**
		 * Executed on plugin initialization.
		 */
		init: function () {
			this.$element = $('<div class="aloha-plugin-htmlsource">');
			this.createEditor();
			this.createDialog();
			this.createButton();
		},

		/**
		 * Create the editor.
		 * @see http://codemirror.net/doc/compress.html (codemirror.js, css.js, javascript.js, xml.js, htmlmixed.js)
		 */
		createEditor: function () {
			var $textarea = $('<textarea>');

			$('<div class="aloha-plugin-htmlsource-container">')
				.append($textarea)
				.appendTo(this.$element);

			this.editor = CodeMirror.fromTextArea($textarea.get(0), {
				mode: 'text/html',
				tabMode: 'indent',
				lineWrapping: true,
				lineNumbers: true,
				autofocus: true
			});
		},

		/**
		 * Create the jQuery UI dialog.
		 */
		createDialog: function () {
			this.$dialog = this.$element.dialog({
				title: 'HTML Source Editor',
				dialogClass: 'aloha-plugin-htmlsource-dialog',
				modal: true,
				width: this.width,
				height: this.height,
				autoOpen: false,
				create: $.proxy(this.onCreate, this),
				open: $.proxy(this.onOpen, this),
				resizeStop: $.proxy(this.onResized, this),
				close: $.proxy(this.onClose, this),
				buttons: {
					'Save': $.proxy(this.onSave, this)
				}
			});
		},

		/**
		 * Create button to display the dialog.
		 */
		createButton: function () {
			var self = this;

			Ui.adopt("htmlsource", Button, {
				tooltip: 'Open HTML Source Editor',
				icon: 'aloha-icon aloha-plugin-htmlsource-button',
				scope: 'Aloha.continuoustext',
				click: function () {
					self.$dialog.dialog('open');
				}
			});
		},

		/**
		 * Resize the editor according to the dialog size.
		 */
		resizeEditor: function () {
			this.editor.setSize(
				this.$dialog.width(),
				this.$dialog.height()
			);
			this.editor.refresh();
		},

		/**
		 * Called when the dialog is created.
		 * @param  {jQuery.Event} e
		 * @param  {Object} ui
		 */
		onCreate: function (e, ui) {
			var self = this;

			$('<input type="checkbox" name="wraped" id="wraped" checked="checked" /><label for="wraped">Word wrap</label>').click(function () {
				self.editor.setOption('lineWrapping', $(this).is(':checked'));
			}).appendTo($(e.target).parent().find('.ui-dialog-buttonpane'));
		},

		/**
		 * Called when opening the dialog.
		 * @param  {jQuery.Event} e
		 * @param  {Object} ui
		 */
		onOpen: function (e, ui) {
			// scroll fix
			this.overflow = $('body').css('overflow');
			$('body').css('overflow', 'hidden');

			this.editable = Aloha.getActiveEditable();
			this.editor.setValue(htmlBeautifier(this.editable.getContents()));
			this.resizeEditor();
		},

		/**
		 * Called when the dialog has been resized.
		 * @param  {jQuery.Event} e
		 * @param  {Object} ui
		 */
		onResized: function (e, ui) {
			this.resizeEditor();
		},

		/**
		 * Called when closing the dialog.
		 * @param  {jQuery.Event} e
		 * @param  {Object} ui
		 */
		onClose: function (e, ui) {
			$('body').css('overflow', this.overflow);
		},

		/**
		 * Called when saving editor content.
		 */
		onSave: function () {
			this.editable.activate();
			this.editable.setContents(this.editor.getValue());
			this.$dialog.dialog('close');
		}

	});

});
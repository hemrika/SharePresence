using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Security;
using System.Reflection;
using System.Globalization;
using System.Collections.Specialized;
using System.Web.UI;
using System.Security.Cryptography;
using System.IO;
using System.Web.Configuration;

namespace Hemrika.SharePresence.Html5.Handlers
{
    /// <summary>
    /// Changes some of functionalities of <see cref="System.Web.Handlers.ScriptResourceHandler" /> to meet html 5
    /// </summary>
    public class ScriptResourceHandler : System.Web.Handlers.ScriptResourceHandler
    {
        #region Const Fields
        private const string OnFormSubmit = @"var _textTypes = /^(text|password|hidden|search|tel|url|email|number|range|color|datetime|date|month|week|time|datetime-local)$/i;
function Sys$WebForms$PageRequestManager$_onFormSubmit(evt) {
        var i, l, continueSubmit = true,
            isCrossPost = this._isCrossPost;
        this._isCrossPost = false;
        if (this._onsubmit) {
            continueSubmit = this._onsubmit();
        }
        if (continueSubmit) {
            for (i = 0, l = this._onSubmitStatements.length; i < l; i++) {
                if (!this._onSubmitStatements[i]()) {
                    continueSubmit = false;
                    break;
                }
            }
        }
        if (!continueSubmit) {
            if (evt) {
                evt.preventDefault();
            }
            return;
        }
        var form = this._form;
        if (isCrossPost) {
            return;
        }
        if (this._activeDefaultButton && !this._activeDefaultButtonClicked) {
            this._onFormElementActive(this._activeDefaultButton, 0, 0);
        }
        if (!this._postBackSettings || !this._postBackSettings.async) {
            return;
        }
        var formBody = new Sys.StringBuilder(),
            count = form.elements.length,
            panelID = this._createPanelID(null, this._postBackSettings);
        formBody.append(panelID);
        for (i = 0; i < count; i++) {
            var element = form.elements[i];
            var name = element.name;
            if (typeof(name) === 'undefined' || (name === null) || (name.length === 0) || (name === this._scriptManagerID)) {
                continue;
            }
            var tagName = element.tagName.toUpperCase();
            if (tagName === 'INPUT') {
                var type = element.type;
                if (_textTypes.test(type) ||
                    (((type === 'checkbox') || (type === 'radio')) && element.checked)) {
                    formBody.append(encodeURIComponent(name));
                    formBody.append('=');
                    formBody.append(encodeURIComponent(element.value));
                    formBody.append('&');
                }
            }
            else if (tagName === 'SELECT') {
                var optionCount = element.options.length;
                for (var j = 0; j < optionCount; j++) {
                    var option = element.options[j];
                    if (option.selected) {
                        formBody.append(encodeURIComponent(name));
                        formBody.append('=');
                        formBody.append(encodeURIComponent(option.value));
                        formBody.append('&');
                    }
                }
            }
            else if (tagName === 'TEXTAREA') {
                formBody.append(encodeURIComponent(name));
                formBody.append('=');
                formBody.append(encodeURIComponent(element.value));
                formBody.append('&');
            }
        }
        formBody.append('__ASYNCPOST=true&');
        if (this._additionalInput) {
            formBody.append(this._additionalInput);
            this._additionalInput = null;
        }
        
        var request = new Sys.Net.WebRequest();
        var action = form.action;
        if (Sys.Browser.agent === Sys.Browser.InternetExplorer) {
            var fragmentIndex = action.indexOf('#');
            if (fragmentIndex !== -1) {
                action = action.substr(0, fragmentIndex);
            }
            var queryIndex = action.indexOf('?');
            if (queryIndex !== -1) {
                var path = action.substr(0, queryIndex);
                if (path.indexOf('%') === -1) {
                    action = encodeURI(path) + action.substr(queryIndex);
                }
            }
            else if (action.indexOf('%') === -1) {
                action = encodeURI(action);
            }
        }
        request.set_url(action);
        request.get_headers()['X-MicrosoftAjax'] = 'Delta=true';
        request.get_headers()['Cache-Control'] = 'no-cache';
        request.set_timeout(this._asyncPostBackTimeout);
        request.add_completed(Function.createDelegate(this, this._onFormSubmitCompleted));
        request.set_body(formBody.toString());
        var panelsToUpdate, eventArgs, handler = this._get_eventHandlerList().getHandler('initializeRequest');
        if (handler) {
            panelsToUpdate = this._postBackSettings.panelsToUpdate;
            eventArgs = new Sys.WebForms.InitializeRequestEventArgs(request, this._postBackSettings.sourceElement, panelsToUpdate);
            handler(this, eventArgs);
            continueSubmit = !eventArgs.get_cancel();
        }
        if (!continueSubmit) {
            if (evt) {
                evt.preventDefault();
            }
            return;
        }
        
        if (eventArgs && eventArgs._updated) {
            panelsToUpdate = eventArgs.get_updatePanelsToUpdate();
            request.set_body(request.get_body().replace(panelID, this._createPanelID(panelsToUpdate, this._postBackSettings)));
        }
        this._scrollPosition = this._getScrollPosition();
        this.abortPostBack();
        handler = this._get_eventHandlerList().getHandler('beginRequest');
        if (handler) {
            eventArgs = new Sys.WebForms.BeginRequestEventArgs(request, this._postBackSettings.sourceElement,
                panelsToUpdate || this._postBackSettings.panelsToUpdate);
            handler(this, eventArgs);
        }
        
        if (this._originalDoCallback) {
            this._cancelPendingCallbacks();
        }
        this._request = request;
        this._processingRequest = false;
        request.invoke();
        if (evt) {
            evt.preventDefault();
        }
    }";
        #endregion
        /// <summary>
        /// processes HTTP Web requests for a script file that is embedded as a resource in an assembly.
        /// </summary>
        /// <param name="context">An System.Web.HttpContext object that provides references to the intrinsic
        /// server objects (for example, Request, Response, Session, and Server) that
        /// are used to service HTTP requests.
        /// </param>
        protected sealed override void ProcessRequest(HttpContext context)
        {
            base.ProcessRequest(context);
            if (CypherContainsAjax(context.Request.QueryString["d"]))
                context.Response.Write(OnFormSubmit);
        }

        private bool CypherContainsAjax(string cypher)
        {
            var text = DecryptString(cypher);
            if (text == null)
                return true; //Then Add it everywhere. What else could I do? :D
            return text.Contains("MicrosoftAjaxWebForms");
        }

        private string DecryptString(string cypher)
        {
            if (PageDecryptString == null)
                return null;
            return (string)PageDecryptString.Invoke(null, new object[] { cypher });
        }
        private static MethodInfo PageDecryptString;
        static ScriptResourceHandler()
        {
            PageDecryptString = typeof(Page).GetMethod("DecryptString", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        }
    }

}

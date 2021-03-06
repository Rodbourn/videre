﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Videre.Core.Extensions;
using Videre.Core.Extensions.Bootstrap.Controls;

namespace Videre.Core.Extensions.Bootstrap
{
    public partial class Bootstrap<TModel>
    {
        public HtmlHelper<TModel> Html;

        public Bootstrap(HtmlHelper<TModel> _html)
        {
            this.Html = _html;
        }

        public IBootstrapButton Button(string id = null)
        {
            return new BootstrapButton(Html, id);
        }

        public IBootstrapAnchorButton AnchorButton(string id = null)
        {
            return new BootstrapAnchorButton(Html, id);
        }

        public IBootstrapRadioButton RadioButton(string id = null)
        {
            return new BootstrapRadioButton(Html, id);
        }

        public IBootstrapSpan Span(string id = null)
        {
            return new BootstrapSpan(Html, id);
        }

        public IBootstrapParagraph Paragraph(string id = null)
        {
            return new BootstrapParagraph(Html, id);
        }

        public IBootstrapUnorderedList UnorderedList(string id = null)
        {
            return new BootstrapUnorderedList(Html, id);
        }

        public IBootstrapLabel Label(string token, string text, string forId)
        {
            return new BootstrapLabel(Html, token, text, forId);
        }
        public IBootstrapLabel Label(string token, string text)
        {
            return new BootstrapLabel(Html, token, text);
        }
        public IBootstrapLabel Label()
        {
            return new BootstrapLabel(Html);
        }
        public IBootstrapTextBox TextBox(string id = null)
        {
            return new BootstrapTextBox(Html, id);
        }
        public IBootstrapCheckBox CheckBox(string id = null)
        {
            return new BootstrapCheckBox(Html, id);
        }
        public IBootstrapTextBox Email(string id = null)
        {
            return new BootstrapTextBox(Html, id).DataType("email").HtmlAttributes(new { type = "email" });
        }
        public IBootstrapDateTimePicker DatePicker(string id = null, string plugin = null)
        {
            plugin = plugin ?? BootstrapDateTimePickerModel.Plugin.BootstrapDateTimePicker.GetDescription();
            return new BootstrapDateTimePicker(Html, id).DataType("date").Plugin(plugin);
        }
        public IBootstrapDateTimePicker DateTimePicker(string id = null, string plugin = null)
        {
            plugin = plugin ?? BootstrapDateTimePickerModel.Plugin.BootstrapDateTimePicker.GetDescription();
            return new BootstrapDateTimePicker(Html, id).DataType("datetime").Plugin(plugin);
        }
        public IBootstrapDateTimePicker TimePicker(string id = null, string plugin = null)
        {
            plugin = plugin ?? BootstrapDateTimePickerModel.Plugin.BootstrapDateTimePicker.GetDescription();
            return new BootstrapDateTimePicker(Html, id).DataType("time").Plugin(plugin);
        }

        public IBootstrapDropDownList TagsInput(string id = null, string plugin = null)
        {
            plugin = plugin ?? BootstrapDropDownListModel.Plugin.BootstrapTagsInput.GetDescription();
            return new BootstrapDropDownList(Html, id).Plugin(plugin).Multiple(); //DONT DO THIS.DataAttribute("role", "tagsinput");
        }

        public IBootstrapTextEditor TextEditor(string id = null, string plugin = null)
        {
            plugin = plugin ?? Services.Portal.CurrentPortal.GetAttribute("Core", "TextEditor", "").ToLower();
            return new BootstrapTextEditor(Html, id).Plugin(plugin);
        }

        public IBootstrapTextBox FileBrowser(string id = null, string mimeType = null)
        {
            //ONLY REGISTER ONCE AND AT END?!?!
            Html.RenderWidget("Core/Admin/FileBrowser", new Dictionary<string, object>() { { "MimeType", mimeType } }, true);

            return new BootstrapTextBox(Html, id).ControlType("filebrowser-input").Append(Button().Icon("glyphicon glyphicon-picture").DataAttribute("action", "filebrowser"));
        }

        public IBootstrapTextArea TextArea(string id = null)
        {
            return new BootstrapTextArea(Html, id);
        }
        public IBootstrapPassword Password(string id = null)
        {
            return new BootstrapPassword(Html, id);
        }
        public IBootstrapUpload Upload(string id = null)
        {
            return new BootstrapUpload(Html, id);
        }

        public IBootstrapDropDownList DropDownList(string id = null)
        {
            return new BootstrapDropDownList(Html, id);
        }

        public IBootstrapDropDownList RoleList(string id = null, List<string> selectedRoles = null)
        {
            selectedRoles = selectedRoles == null ? new List<string>() : selectedRoles;
            var items = Services.Account.GetRoles().Select(r => new SelectListItem() { Value = r.Id, Text = r.Name, Selected = selectedRoles.Contains(r.Id) }).ToList();
            return DropDownList(id).Options(items);
        }

        public IBootstrapDropDownList ClaimList(string id = null, List<Models.UserClaim> selectedClaims = null)
        {
            selectedClaims = selectedClaims == null ? new List<Models.UserClaim>() : selectedClaims;
            var items = Services.Account.GetClaims().Select(r => new SelectListItem() { Value = string.Format("{0}:{1}:{2}", r.Issuer, r.Type, r.Value), Text = string.Format("{0}:{1}:{2}", r.Issuer, r.Type, r.Value), 
                Selected = selectedClaims.Exists(s => s.Issuer.Equals(r.Issuer, StringComparison.CurrentCultureIgnoreCase) && 
                                                s.Type.Equals(r.Type, StringComparison.CurrentCultureIgnoreCase) && 
                                                s.Value.Equals(r.Value, StringComparison.CurrentCultureIgnoreCase))}).ToList();
            this.Html.RegisterWebReferenceGroup("bootstrap-select");
            return DropDownList(id).Options(items).Append(Button().Icon("glyphicon glyphicon-plus").DataAttribute("action", "addclaim")).ControlType("claims-list");
        }

        public IBootstrapDropDownList CacheKeyList(string id = null, List<string> selectedKeys = null)
        {
            selectedKeys = selectedKeys == null ? new List<string>() : selectedKeys;
            var items = Services.Widget.GetWidgetCacheKeyProviderDict().Keys.Select(k => new SelectListItem() { Value = k, Text = k, Selected = selectedKeys.Contains(k) }).ToList();
            this.Html.RegisterWebReferenceGroup("bootstrap-select");
            return DropDownList(id).Options(items);
        }

        public IBootstrapDropDownList AuthorizedList(string id = null)
        {
            var clientControl = Html.ViewData.Model as Videre.Core.Models.IClientControl;
            if (clientControl != null)
            {
                return DropDownList(id).Options(new List<SelectListItem>()
                {
                    new SelectListItem {Text = clientControl.GetPortalText("None.Text", "None"), Value = ""},
                    new SelectListItem {Text = clientControl.GetPortalText("Authenticated.Text", "Authenticated"), Value = "true"},
                    new SelectListItem {Text = clientControl.GetPortalText("NotAuthenticated.Text", "Not Authenticated"), Value = "false"}
                });
            }
            else
                throw new Exception("No Client control found to optain text from");
        }


        public IBootstrapFormGroup FormGroup(IBootstrapLabel label, IBootstrapBaseControl control)
        {
            return new BootstrapFormGroup(Html, label, control);
        }
        public IBootstrapFormGroup FormGroup(IBootstrapLabel label, IBootstrapBaseControl control, BootstrapUnits.GridSize controlSize)
        {
            return new BootstrapFormGroup(Html, label, control, controlSize);
        }

        public IBootstrapFormGroup FormGroup(IBootstrapLabel label, List<IBootstrapBaseControl> controls)
        {
            return new BootstrapFormGroup(Html, label, controls);
        }

        public IBootstrapFormGroup FormGroup(IBootstrapLabel label, List<IBootstrapBaseControl> controls, BootstrapUnits.GridSize size)
        {
            return new BootstrapFormGroup(Html, label, controls, size);
        }

        public IBootstrapTabs Tabs(List<Bootstrap.Controls.BootstrapTab> tabs = null)
        {
            return new BootstrapTabs(Html).Tabs(tabs);
        }

        //public BootstrapLabel LabelFor<TValue>(Expression<Func<TModel, TValue>> expression)
        //{
        //    return new BootstrapLabel(Html, ExpressionHelper.GetExpressionText(expression), ModelMetadata.FromLambdaExpression(expression, Html.ViewData));
        //}

    }
}

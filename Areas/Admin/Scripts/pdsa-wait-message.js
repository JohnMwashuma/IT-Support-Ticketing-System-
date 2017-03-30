/* ************************************ */
/* BEGIN: Submit Message Display Script */
/* ************************************ */
function DisplayProgressMessage(ctl, msg) {
  // Turn off the "Save" button and change text
  // NOTE: This does not work in ASP.NET Web Forms
  //       Instead you have to addClass('disabled')
  $(ctl).prop("disabled", true).text(msg);

  // Wrap in setTimeout in case the UI needs to update any spinners, etc.
  setTimeout(function () {
    // Only display wait message and dim background 
    // if 'data-pdsa-show-on-save' attribute is present
    if ($("[data-pdsa-show-on-progress='true']").length > 0) {
      $("[data-pdsa-show-on-progress='true']").removeClass("hidden").removeClass("hide");
      $("body").addClass("submit-progress-bg");
    }
  }, 1);

  return true;
};
/* ********************************** */
/* END: Submit Message Display Script */
/* ********************************** */
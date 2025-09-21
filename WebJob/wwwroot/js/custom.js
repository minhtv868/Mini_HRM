
$(document).on("change", "#LeagueId", function () {
    var selectedText = $("#LeagueId option:selected").text();
    $("#LeagueName").val(selectedText);
});
$(document).on("change", "#TeamHomeId", function () {
    var selectedText = $("#TeamHomeId option:selected").text();
    $("#TeamHomeName").val(selectedText);
});
$(document).on("change", "#TeamAwayId", function () {
    var selectedText = $("#TeamAwayId option:selected").text();
    $("#TeamAwayName").val(selectedText);
});

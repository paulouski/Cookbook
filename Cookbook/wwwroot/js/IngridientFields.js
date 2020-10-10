var data = $('#ingredients').attr('data-count') || 0;
var counter = parseInt(data);
$("#addIngredient").click(function () {
    if (counter < 0) {
        counter = 0;
    }
    $("#ingredients").append(
        '<div class="col-md-12" id=p' + counter + '>' +
        '<div class="col-md-3 control-label"><b>Имя ингредиента #' + (counter + 1) + '</b></div>' +
        '<div class="col-md-3"><input class="form-control valid" type="text" id="IngredientsForRecipe_' + counter + '__IngredientName" name="IngredientsForRecipe[' + counter + '].IngredientName" value="" aria-invalid="false"></div>' +
        '<div class="col-md-3 control-label"><b>Количество ингредиента #' + (counter + 1) + '</b></div>' +
        '<div class="col-md-3"><input class="form-control valid" type="text" id="IngredientsForRecipe_' + counter + '__Amount" name="IngredientsForRecipe[' + counter + '].Amount" value="" aria-invalid="false"></div>' +
        '</div>'
    );
    counter++;
});
$("#delIngredient").click(function () {
    $("#p" + (counter - 1)).remove();
    counter--;
});
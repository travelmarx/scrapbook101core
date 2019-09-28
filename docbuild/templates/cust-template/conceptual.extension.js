// Copyright (c) Microsoft. All rights reserved. Licensed under the MIT license. See LICENSE file in the project root for full license information.

/**
 * This method will be called at the start of exports.transform in conceptual.html.primary.js
 */
exports.preTransform = function (model) {
  return model;
}

String.prototype.replaceAll = function(search, replacement) {
  var target = this;
  return target.split(search).join(replacement);
};

/**
 * This method will be called at the end of exports.transform in conceptual.html.primary.js
 */
exports.postTransform = function (model) {
  model.conceptual = model.conceptual.replaceAll("{{productName}}", model["productName"] );
  return model;
}

export default {
  orkNodes: getOrks()
}

function getOrks() {
  var list = [];
  for (var i = 1; i < 26; i++) {
    list.push(`https://raziel-ork-${i}.azurewebsites.net/`);
  }
  return list;
}

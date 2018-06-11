
export function generateUuid(): string {
  var d = new Date().getTime();
  if (typeof performance !== 'undefined' && typeof performance.now === 'function'){
    d += performance.now(); //use high-precision timer if available
  }
  return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
    var r = (d + Math.random() * 16) % 16 | 0;
    d = Math.floor(d / 16);
    return (c === 'x' ? r : (r & 0x3 | 0x8)).toString(16);
  });
}

export function defaultUserUuid(): string {
  return '39488374-2993-3948-5938-394883720428';
}

export function emptyUuid(): string {
  return 'xxxxxxxx-xxxx-xxxx-yxxx-xxxxxxxxxxxx';
}

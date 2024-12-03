export function storageSet<T>(key: string, data: T) {
    localStorage.setItem(key, JSON.stringify(data));
}

export function storageGet<T>(key: string) {
    const stringData = localStorage.getItem(key);
    if (stringData === null)
        return [] as T
  return JSON.parse(stringData);
}
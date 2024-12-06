import { defineStore } from 'pinia'


type IApplicationState = {
    cacheValue: string
    currentKey: string
}  

export const useApplication = defineStore('applicationStore', {
    state: ():IApplicationState  => ({ cacheValue: '', currentKey: '' }),
    actions: {
        setCurrentCacheValue(value: string) {
            this.cacheValue = value
        },
        setCurrentKey(value: string) {
            this.currentKey = value
        }
    }
})
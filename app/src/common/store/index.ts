import { defineStore } from 'pinia'


type IApplicationState = {
    cacheValue?: string
}  

export const useApplication = defineStore('applicationStore', {
    state: ():IApplicationState  => ({ cacheValue: '' }),
    actions: {
        setCurrentCacheValue(value: string) {
            this.cacheValue = value
        }
    }
})
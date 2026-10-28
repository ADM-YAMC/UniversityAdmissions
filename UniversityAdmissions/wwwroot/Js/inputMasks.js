
window.inputMasks = {
    instances: {},
    phoneInstances: {},
    apply: function (elementId, type) {
        const element = document.getElementById(elementId);
        if (!element || !window.IMask) return;
        if (this.instances[elementId]) {
            this.instances[elementId].destroy();
            delete this.instances[elementId];
        }
        let options;
        switch (type) {
            case "cedula":
                options = { mask: "000-0000000-0" };
                break;
            case "passport":
                options = { mask: /^[a-zA-Z0-9]*$/ };
                break;
            default:
                return;
        }
        this.instances[elementId] = IMask(element, options);
        this.instances[elementId].on("accept", () => {
            element.dispatchEvent(new Event("change", { bubbles: true }));
        });
    },
    applyPhone: function (elementId) {
        const element = document.getElementById(elementId);
        if (!element || !window.intlTelInput) return;
        if (this.phoneInstances[elementId]) return;
        const iti = window.intlTelInput(element, {
            initialCountry: "do",
            preferredCountries: ["do", "us", "es", "mx", "co"],
            nationalMode: true,
            autoPlaceholder: "polite",
            formatAsYouType: true,
            separateDialCode: true,
            loadUtils: () => import("https://cdn.jsdelivr.net/npm/intl-tel-input@25.12.5/build/js/utils.js")
        });
        this.phoneInstances[elementId] = iti;
    },
    getPhoneNumber: function (elementId) {
        const iti = this.phoneInstances[elementId];
        return iti ? iti.getNumber() : "";
    },
    isValidPhone: function (elementId) {
        const iti = this.phoneInstances[elementId];
        return iti ? iti.isValidNumber() : false;
    },
    getSelectedCountry: function (elementId) {
        const iti = this.phoneInstances[elementId];
        return iti ? iti.getSelectedCountryData() : null;
    },
    getIdentification: function (elementId) {
        const instance = this.instances[elementId];
        if (instance) return instance.value.trim();
        const element = document.getElementById(elementId);
        return element ? element.value.trim() : "";
    },
    isValidIdentification: function (elementId) {
        const element = document.getElementById(elementId);
        if (!element) return false;
        const instance = this.instances[elementId];
        if (instance) {
            if (instance.masked.isComplete === false) return false;
            return instance.value.trim().length > 0;
        }
        return element.value.trim().length > 0;
    },
    getIdentificationUnmasked: function (elementId) {
        const instance = this.instances[elementId];
        if (instance) return instance.unmaskedValue.trim();
        const element = document.getElementById(elementId);
        return element ? element.value.trim() : "";
    },
    destroy: function (elementId) {
        if (this.instances[elementId]) {
            this.instances[elementId].destroy();
            delete this.instances[elementId];
        }
        if (this.phoneInstances[elementId]) {
            this.phoneInstances[elementId].destroy();
            delete this.phoneInstances[elementId];
        }
    }
};

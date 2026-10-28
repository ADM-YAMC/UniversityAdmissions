
window.admissionAlerts = {
    show: async function (options = {}) {
        const {
            icon = "info",
            title = "Notificación",
            text = "",
            confirmButtonText = "Aceptar",
            confirmButtonColor = "#2A73C6",
            showCancelButton = false,
            cancelButtonText = "Cancelar",
            cancelButtonColor = "#64748B",
            allowOutsideClick = false,
            allowEscapeKey = true,
            timer = null,
            showConfirmButton = true
        } = options;
        if (!window.Swal) {
            alert(`${title}\n${text}`);
            return { isConfirmed: true, isDismissed: false };
        }
        return await Swal.fire({
            icon,
            title,
            text,
            confirmButtonText,
            confirmButtonColor,
            showCancelButton,
            cancelButtonText,
            cancelButtonColor,
            allowOutsideClick,
            allowEscapeKey,
            timer,
            showConfirmButton,
            reverseButtons: true,
            buttonsStyling: true
        }).then(result => ({
            isConfirmed: result.isConfirmed,
            isDismissed: result.isDismissed
        }));
    },
    success: async function (title, message) {
        return await this.show({
            icon: "success",
            title,
            text: message
        });
    },
    error: async function (title, message) {
        return await this.show({
            icon: "error",
            title,
            text: message
        });
    },
    warning: async function (title, message) {
        return await this.show({
            icon: "warning",
            title,
            text: message
        });
    },
    info: async function (title, message) {
        return await this.show({
            icon: "info",
            title,
            text: message
        });
    },
    confirm: async function (title, message) {
        const result = await this.show({
            icon: "question",
            title,
            text: message,
            showCancelButton: true,
            confirmButtonText: "Sí, continuar",
            cancelButtonText: "Cancelar"
        });
        return result.isConfirmed;
    }
};

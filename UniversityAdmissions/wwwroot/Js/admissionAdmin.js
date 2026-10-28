window.admissionAdmin = {
    confirm: async function (title, message) {
        if (typeof Swal === "undefined") return window.confirm(message);
        const result = await Swal.fire({ title, text: message, icon: "question", showCancelButton: true, confirmButtonText: "Confirmar", cancelButtonText: "Cancelar", confirmButtonColor: "#1D4F95" });
        return result.isConfirmed;
    },
    download: async function (name, streamReference) {
        const arrayBuffer = await streamReference.arrayBuffer();
        const url = URL.createObjectURL(new Blob([arrayBuffer]));
        const link = document.createElement("a");
        link.href = url;
        link.download = name;
        document.body.appendChild(link);
        link.click();
        link.remove();
        setTimeout(() => URL.revokeObjectURL(url), 1000);
    }
};

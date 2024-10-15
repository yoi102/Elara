﻿using Service.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Elara.wpf.Assists
{
    internal class ShowProgressBarDisposable : IDisposable
    {
        private readonly IDialogService dialogService;
        private readonly object dialogIdentifier;

        public ShowProgressBarDisposable(IDialogService dialogService, object dialogIdentifier)
        {
            this.dialogService = dialogService;
            this.dialogIdentifier = dialogIdentifier;

            dialogService.TryShowProgressBarDialog(dialogIdentifier);
        }


        public void Dispose()
        {
            dialogService.TryCloseDialog(dialogIdentifier);
        }
    }
}

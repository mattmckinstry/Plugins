﻿using AnyStatus.API;
using System.Threading.Tasks;
using System.Windows;

namespace AnyStatus
{
    public class TriggerJenkinsJob : ITriggerBuild<JenkinsJob_v1>
    {
        private readonly ILogger _logger;
        private readonly IDialogService _dialogService;
        private readonly IJenkinsClient _jenkinsClient;

        public TriggerJenkinsJob(IDialogService dialogService, ILogger logger, IJenkinsClient jenkinsClient)
        {
            _logger = Preconditions.CheckNotNull(logger, nameof(logger));
            _dialogService = Preconditions.CheckNotNull(dialogService, nameof(dialogService));
            _jenkinsClient = Preconditions.CheckNotNull(jenkinsClient, nameof(jenkinsClient));
        }

        public async Task HandleAsync(JenkinsJob_v1 jenkinsJob)
        {
            var result = _dialogService.Show(
                $"Are you sure you want to trigger {jenkinsJob.Name}?",
                "Trigger a new build",
                MessageBoxButton.YesNo,
                MessageBoxImage.Asterisk);

            if (result != MessageBoxResult.Yes) return;

            await _jenkinsClient.TriggerJobAsync(jenkinsJob).ConfigureAwait(false);

            _logger.Info($"Jenkins Job \"{jenkinsJob.Name}\" has been triggered.");
        }
    }
}
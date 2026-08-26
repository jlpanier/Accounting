using Business;
using Syncfusion.Maui.DataSource.Extensions;
using System.Collections.ObjectModel;
using System.Windows.Input;
using static Business.Order;

namespace Main.ViewModels
{
    public class MonthlyPeaViewModel : BaseViewModel
    {
        /// <summary>
        /// Evenement pour édition du compte
        /// </summary>
        public ICommand ClickNewTransferCommand => new Command(OnNewTransfer);

        /// <summary>
        /// Evenement pour achat d'une action
        /// </summary>
        public ICommand ClickNewPurchaseCommand => new Command(OnPurchase);

        /// <summary>
        /// Evenement pour la vente d'une action
        /// </summary>
        public ICommand ClickNewSellCommand => new Command(OnSell);

        /// <summary>
        /// Evenement pour la vente d'une action
        /// </summary>
        public ICommand ClickNewDividendeCommand => new Command(OnDividente);

        /// <summary>
        /// Evenement pour la modification d'une action
        /// </summary>
        public ICommand ClickNameCommand => new Command<int>(OnShare);

        /// <summary>
        /// Evenement pour la modification du montant d'une action
        /// </summary>
        public ICommand ClickMonthlyCommand => new Command<int>(OnMonthlyShare);

        /// <summary>
        /// Evenement pour la modification du montant d'une action
        /// </summary>
        public ICommand ClickTransactionCommand => new Command<LineTransactions>(OnTransaction);

        /// <summary>
        /// Label de la période 
        /// </summary>
        public string MonthLabel
        {
            get => _monthLabel;
            set
            {
                if (_monthLabel != value)
                {
                    _monthLabel = value;
                    NotifyPropertyChanged(nameof(MonthLabel));
                }
            }
        }
        private string _monthLabel = "";

        /// <summary>
        /// Label de la période 
        /// </summary>
        public string PeriodLabel
        {
            get => _periodLabel;
            set
            {
                if (_periodLabel != value)
                {
                    _periodLabel = value;
                    NotifyPropertyChanged(nameof(PeriodLabel));
                }
            }
        }
        private string _periodLabel = "";

        /// <summary>
        /// Référence du compte
        /// </summary>
        public int BankAccountId;

        /// <summary>
        /// Date d'affichage de la période 
        /// </summary>
        public DateTime EffectiveOn
        {
            get => _effectiveOn;
            set
            {
                if (_effectiveOn != value)
                {
                    _effectiveOn = value;
                    MonthLabel = _effectiveOn.ToString("MMMM yyyy", new System.Globalization.CultureInfo("fr-FR"));
                    PeriodLabel = $"{_effectiveOn.AddMonths(-1).ToString("dd/MM/yyyy")} \u2192 {_effectiveOn.ToString("dd/MM/yyyy")}";
                    NotifyPropertyChanged(nameof(EffectiveOn));
                }
            }
        }
        private DateTime _effectiveOn;

        /// <summary>
        /// Liste des actions déjà acquises
        /// </summary>
        public ObservableCollection<Business.MonthlyShare> Shares
        {
            get => _shares;
            set
            {
                if (_shares != value)
                {
                    _shares = value;
                    NotifyPropertyChanged(nameof(Shares));
                }
            }
        }
        private ObservableCollection<Business.MonthlyShare> _shares = new ObservableCollection<Business.MonthlyShare>();

        /// <summary>
        /// Liste des transactions avec solde 
        /// /// </summary>
        public ObservableCollection<LineTransactions> LineTransactions
        {
            get => _linetransactions;
            set
            {
                if (_linetransactions != value)
                {
                    _linetransactions = value;
                    NotifyPropertyChanged(nameof(LineTransactions));
                }
            }
        }
        private ObservableCollection<LineTransactions> _linetransactions = new ObservableCollection<LineTransactions>();

        public MonthlyPeaViewModel()
        {
        }

        /// <summary>
        /// Initialisation des données
        /// </summary>
        public void Init(int bankAccountId, DateTime effectiveOn)
        {
            BankAccountId = bankAccountId;
            EffectiveOn = effectiveOn;
            var item = BankAccount.GetById(bankAccountId);
            if (item is PEA bankaccount)
            {
                Shares = new ObservableCollection<Business.MonthlyShare>(bankaccount.ShareOn(effectiveOn).OrderBy(_=>_.Type).ThenBy(_=>_.Label));

                double solde = 0;
                var lines = new List<LineTransactions>();
                foreach (var transaction in bankaccount.Transactions.OrderBy(_ => _.EffectiveOn))
                {
                    solde += transaction.Amount;
                    lines.Add(new Business.LineTransactions(transaction, solde));
                }
                LineTransactions = new ObservableCollection<LineTransactions>(lines.Where(_ => _.EffectiveOn > DateTime.Now.AddYears(-1)).OrderByDescending(_ => _.EffectiveOn));
            }
        }

        /// <summary>
        /// Ecran d'jout d'une nouvelle opérations
        /// </summary>
        private async void OnNewTransfer()
        {
            await Shell.Current.GoToAsync($"{nameof(EditTransferPeaPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }

        /// <summary>
        /// Ecran d'un nouvel order d'achat
        /// </summary>
        private async void OnPurchase()
        {
            await Shell.Current.GoToAsync($"{nameof(EditPurchasePeaPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }

        /// <summary>
        /// Ecran d'une vente d'action
        /// </summary>
        private async void OnSell()
        {
            await Shell.Current.GoToAsync($"{nameof(EditSellPeaPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }

        /// <summary>
        /// Ecran d'une vente d'action
        /// </summary>
        private async void OnDividente()
        {
            await Shell.Current.GoToAsync($"{nameof(EditDividendePeaPage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
            });
        }

        /// <summary>
        /// Ecran d'une vente d'action
        /// </summary>
        private async void OnShare(int shareId)
        {
            await Shell.Current.GoToAsync($"{nameof(SharePage)}", new Dictionary<string, object>
            {
                ["shareId"] = shareId,
            });
        }

        /// <summary>
        /// Ecran d'une vente d'action
        /// </summary>
        private async void OnMonthlyShare(int shareId)
        {
            await Shell.Current.GoToAsync($"{nameof(EditMonthSharePricePage)}", new Dictionary<string, object>
            {
                ["BankAccountId"] = BankAccountId,
                ["EffectiveOn"] = EffectiveOn,
                ["ShareId"] = shareId,
            });
        }

        /// <summary>
        /// Ecran d'une vente d'action
        /// </summary>
        private async void OnTransaction(LineTransactions transaction)
        {
            if (transaction.Transaction is Transfer transfer)
            {
                await Shell.Current.GoToAsync($"{nameof(EditTransferPeaPage)}", new Dictionary<string, object>
                {
                    ["BankAccountId"] = BankAccountId,
                    ["EffectiveOn"] = EffectiveOn,
                    ["Key"] = transaction.Id,
                });
            }
            else if (transaction.Transaction is Order order)
            {
                if (order.Type == OrderType.Buy)
                {
                    await Shell.Current.GoToAsync($"{nameof(EditPurchasePeaPage)}", new Dictionary<string, object>
                    {
                        ["BankAccountId"] = BankAccountId,
                        ["EffectiveOn"] = EffectiveOn,
                        ["Key"] = transaction.Id,
                    });
                }
                else if (order.Type == OrderType.Sell)
                {
                    await Shell.Current.GoToAsync($"{nameof(EditSellPeaPage)}", new Dictionary<string, object>
                    {
                        ["BankAccountId"] = BankAccountId,
                        ["EffectiveOn"] = EffectiveOn,
                        ["Key"] = transaction.Id,
                    });
                }
            }
            else if (transaction.Transaction is Dividende dividende)
            {
                await Shell.Current.GoToAsync($"{nameof(EditDividendePeaPage)}", new Dictionary<string, object>
                {
                    ["BankAccountId"] = BankAccountId,
                    ["EffectiveOn"] = EffectiveOn,
                    ["Key"] = transaction.Id,
                });
            }
        }
    }
}